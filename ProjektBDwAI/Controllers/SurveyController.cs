using Microsoft.AspNetCore.Mvc;

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
    }
}
