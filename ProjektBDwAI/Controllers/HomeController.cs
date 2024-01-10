using Microsoft.AspNetCore.Mvc;
using ProjektBDwAI.Models;
using System.Diagnostics;

namespace ProjektBDwAI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                ViewData["Username"] = HttpContext.Session.GetString("Username");
                ViewData["UserId"] = HttpContext.Session.GetInt32("UserId");
                var userSurveys = _context.Surveys.Where(s => s.OwnerId == HttpContext.Session.GetInt32("UserId")).ToList();
                var publicSurveys = _context.Surveys.Where(s => s.IsPublic).ToList();

                var viewModel = new HomeViewModel
                {
                    UserSurveys = userSurveys,
                    PublicSurveys = publicSurveys
                };
                return View(viewModel);
            }

            return RedirectToAction("Login", "Account");
        }

        public IActionResult Privacy()
        {
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Userpage()
        {
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
