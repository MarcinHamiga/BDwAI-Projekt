using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjektBDwAI.Models;
using System.Security.Policy;

namespace ProjektBDwAI.Controllers
{
    public class SurveyController : Controller
    {
        private readonly ApplicationDbContext _context;

        private Survey getSurvey(int id)
        {
            var Survey = _context.Surveys.FirstOrDefault(s => s.Id == id);
            if (Survey != null)
            {
                return Survey;
            }
            return null;
            
        }

        private int getSurveyOwnerId(int id)
        {
            return _context.Surveys.FirstOrDefault(s => s.Id == id).OwnerId;
        }

        private bool isAdmin()
        {
            if (HttpContext.Session.GetInt32("isAdmin") == 1)
            {
                return true;
            } else
            {
                return false;
            }
        }

        private bool isOwner(int id)
        {
            if (isAdmin())
            {
                return true;
            }
            return getSessionId() == getSurveyOwnerId(id);
        }

        private string getAnswerContentById(int id)
        {
            return _context.Answers.FirstOrDefault(a => a.Id == id).Content.ToString();
        }

        private List<Question> getQuestions(int surveyId)
        {
            var Questions = _context.Questions.Where(q => q.SurveyId == surveyId).ToList();
            return Questions;
        }

        private List<Answer> getAnswers(List<Question> Questions)
        {
            List<Answer> Answers = new List<Answer>();
            foreach(var question in Questions)
            {
                Answers.AddRange(_context.Answers.Where(a => a.QuestionId == question.Id).ToList());
            }

            return Answers;
        }

        private bool checkSession()
        {
            return HttpContext.Session.GetInt32("UserId").HasValue;
        }

        private int getSessionId()
        {
            if (checkSession())
            {
                return HttpContext.Session.GetInt32("UserId").Value;
            }

            return -1;
            
        }

        private string getSessionUsername()
        {
            if (checkSession())
            {
                return HttpContext.Session.GetString("Username");
            }

            return "";
        }

        public SurveyController(ApplicationDbContext appDb)
        {
            _context = appDb;
        }
        public IActionResult Show(int Id)
        {
            if (checkSession())
            {
                SurveyShowModel viewModel = new SurveyShowModel();
                viewModel.Survey = _context.Surveys.FirstOrDefault(s => s.Id == Id);
                viewModel.Questions = getQuestions(Id);
                viewModel.Answers = getAnswers(viewModel.Questions);
                ViewData["isAdmin"] = isAdmin();
                
                return View(viewModel);
            }
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Edit(int Id) 
        {
            if (checkSession() && isOwner(Id)) { 
                SurveyEditModel viewModel = new SurveyEditModel();
                viewModel.Survey = _context.Surveys.FirstOrDefault(s => s.Id == Id);
                viewModel.Questions = getQuestions(viewModel.Survey.Id);
                viewModel.Answers = getAnswers(viewModel.Questions);
                return View(viewModel);
            } else if (checkSession() && !isOwner(Id))
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "Account");
        }
        public IActionResult Publish(int id)
        {
            if (checkSession() && isOwner(id))
            {
                Survey survey = _context.Surveys.FirstOrDefault(s => s.Id == id);
                survey.IsPublic = (!survey.IsPublic);
                _context.SaveChanges();
                return RedirectToAction("Show", new { Id = id });
            } else if (checkSession() && !isOwner(id))
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "Account");

        }

        public IActionResult Delete(int id)
        {
            if (checkSession() && isOwner(id))
            {
                Survey survey = _context.Surveys.FirstOrDefault(s => s.Id == id);
                _context.Surveys.Remove(survey);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            } else if (checkSession() && !isOwner(id))
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "Account");
            
        }

        public IActionResult Result(int id)
        {
            if (checkSession() && isOwner(id))
            {
                List<UserResult> userResults = _context.UserResults
                .Include(ur => ur.Question)
                .Include(ur => ur.Result)
                .Where(ur => ur.Result.SurveyId == id)
                .ToList();

                ViewData["SurveyId"] = id;
                foreach (var result in userResults)
                {
                    if (result.SelectedAnswer != "0")
                    {
                        ViewData[result.SelectedAnswer] = getAnswerContentById(int.Parse(result.SelectedAnswer));
                    }
                }
                var viewModel = new UserResultView
                {
                    UserResult = userResults
                };

                return View(viewModel);
            } else if (checkSession() && !isOwner(id))
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "Account");
            
        }
        [HttpPost]
        public async Task<IActionResult> Edit(SurveyEditModel model)
        {
            if (checkSession() && isOwner(model.surveyId))
            {


                switch (model.Action)
                {
                    case 1:
                        var question = new Question();
                        question.QuestionType = model.questionType;
                        question.SurveyId = model.surveyId;
                        question.Content = model.questionContent;

                        await _context.Questions.AddAsync(question);
                        await _context.SaveChangesAsync();
                        break;
                    case 2:
                        var answer = new Answer();
                        answer.QuestionId = model.questionId;
                        answer.Content = model.answerContent;
                        answer.Value = "A jak pan Marcin powiedzial?";
                        await _context.Answers.AddAsync(answer);
                        await _context.SaveChangesAsync();
                        break;
                    case 3:
                        var questionDel = _context.Questions.Find(model.questionId);
                        if (questionDel != null)
                        {
                            _context.Questions.Remove(questionDel);
                            _context.SaveChanges();
                        }
                        break;
                    case 4:
                        var answerDel = _context.Answers.Find(model.answerId);
                        if (answerDel != null)
                        {
                            _context.Answers.Remove(answerDel);
                            _context.SaveChanges();
                        }
                        break;
                    default:
                        break;
                }

                model.Survey = _context.Surveys.FirstOrDefault(s => s.Id == model.surveyId);
                model.Questions = getQuestions(model.Survey.Id);
                model.Answers = getAnswers(model.Questions);
                model.questionType = null;
                model.questionContent = null;
                return View(model);
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> AddSurvey(Survey model)
        {
            if (checkSession()) {
                int userId = HttpContext.Session.GetInt32("UserId").Value;
                model.OwnerId = userId;
                model.IsPublic = false;
                _context.Surveys.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Userpage", "Home");
                
            }
            return RedirectToAction("Userpage", "Home");
        }

        public IActionResult Fill(int Id)
        {
            if (checkSession() && isOwner(Id))
            {
                SurveyFillModel viewModel = new SurveyFillModel();
                viewModel.Survey = _context.Surveys.FirstOrDefault(s => s.Id == Id);
                if (viewModel.Survey == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                viewModel.Questions = getQuestions(viewModel.Survey.Id);
                viewModel.Answers = getAnswers(viewModel.Questions);
                viewModel.Results = new List<string>();
                foreach (var question in viewModel.Questions)
                {
                    viewModel.Results.Add("");
                }
                ViewData["SurveyId"] = (int)viewModel.Survey.Id;
                ViewData["isAdmin"] = isAdmin();

                return View(viewModel);
            } else if (checkSession() && !isOwner(Id))
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        public async Task<IActionResult> Fill(SurveyFillModel model)
        {
            if (checkSession())
            {
                int x = 0;
                Result Result = new Result {
                    SurveyId = model.surveyId,
                    DateFilled = DateTime.Now
                };

                _context.Results.Add(Result);
                await _context.SaveChangesAsync();

                List<Question> Questions = getQuestions(model.surveyId);

                foreach(var userResult in model.Results)
                {
                    var Question = Questions[x];
                    UserResult UserResult = new UserResult();
                    UserResult.ResultId = Result.Id;
                    UserResult.QuestionId = Question.Id;
                    if (Question.QuestionType == "text")
                    {
                        UserResult.AnswerText = userResult;
                        UserResult.SelectedAnswer = "0";
                    }
                    else
                    {
                        UserResult.AnswerText = "RADIO-TYPE-ANSWER";
                        UserResult.SelectedAnswer = userResult;
                    }

                    x++;

                    await _context.UserResults.AddAsync(UserResult);
                    
                }
                await _context.SaveChangesAsync();


                return RedirectToAction("Show", new { Id = model.surveyId});
            }
            return RedirectToAction("Login", "Account");
        }
    }
}
