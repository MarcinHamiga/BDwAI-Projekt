using Microsoft.AspNetCore.Mvc;
using ProjektBDwAI.Models;

namespace ProjektBDwAI.Controllers
{
    public class SurveyController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SurveyController(ApplicationDbContext appDb)
        {
            _context = appDb;
        }
        public IActionResult Show(int Id)
        {
            SurveyShowModel viewModel = new SurveyShowModel();
            viewModel.Survey = _context.Surveys.FirstOrDefault(s => s.Id == Id);
            var questions = _context.Questions.Where(q => q.SurveyId == Id).ToList();

            foreach (var question in questions)
            {
                var answers = _context.Answers.Where(a => a.QuestionId == question.Id).ToList();
                viewModel.Answers.AddRange(answers);
            }
            viewModel.Questions = questions;

            return View(viewModel);
        }

        public IActionResult Edit(int Id) 
        {
            SurveyEditModel viewModel = new SurveyEditModel();
            viewModel.Survey = _context.Surveys.FirstOrDefault(s => s.Id == Id);
            var questions = _context.Questions.Where(q => q.SurveyId == Id).ToList();

            foreach (var question in questions)
            {
                var answers = _context.Answers.Where(a => a.QuestionId == question.Id).ToList();
                viewModel.Answers.AddRange(answers);
            }
            viewModel.Questions = questions;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SurveyEditModel model)
        {
            switch (model.Action)
            {
                case 1:
                    var question = new Question();
                    question.QuestionType = "text";
                    question.SurveyId = model.Survey.Id;
                    question.Content = model.QuestionContent;

                    await _context.Questions.AddAsync(question);
                    await _context.SaveChangesAsync();
                    break;
                case 2:

                    break;
                case 3:
                    break;
                case 4:
                    break;
                default:
                    break;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddSurvey(Survey model)
        {
            if (HttpContext.Session.GetInt32("UserId").HasValue) {
                int userId = HttpContext.Session.GetInt32("UserId").Value;
                model.OwnerId = userId;
                model.IsPublic = false;
                _context.Surveys.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Userpage", "Home");
                
            }
            return RedirectToAction("Userpage", "Home");
        }
    }
}
