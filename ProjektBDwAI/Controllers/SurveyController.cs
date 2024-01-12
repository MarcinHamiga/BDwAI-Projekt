using Microsoft.AspNetCore.Mvc;
using ProjektBDwAI.Models;

namespace ProjektBDwAI.Controllers
{
    public class SurveyController : Controller
    {
        private readonly ApplicationDbContext _context;

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
                return View(viewModel);
            }
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Edit(int Id) 
        {
            if (checkSession()) { 
                SurveyEditModel viewModel = new SurveyEditModel();
                viewModel.Survey = _context.Surveys.FirstOrDefault(s => s.Id == Id);
                viewModel.Questions = getQuestions(viewModel.Survey.Id);
                viewModel.Answers = getAnswers(viewModel.Questions);
                return View(viewModel);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SurveyEditModel model)
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
                    if(questionDel != null)
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
            if (checkSession())
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

                return View(viewModel);
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
