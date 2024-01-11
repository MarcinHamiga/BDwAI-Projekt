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

            return View();
        }

        public IActionResult Show()
        {
            return RedirectToAction("Index", "Home");
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
