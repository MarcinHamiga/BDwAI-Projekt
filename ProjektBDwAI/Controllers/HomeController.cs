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

        private int getSurveyOwnerId(int id)
        {
            return _context.Surveys.FirstOrDefault(s => s.Id == id).OwnerId;
        }

        private bool isAdmin()
        {
            if (HttpContext.Session.GetInt32("isAdmin") == 1)
            {
                return true;
            }
            else
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
                int userId = HttpContext.Session.GetInt32("UserId").Value;
                ViewData["UserId"] = userId;
                ViewData["isAdmin"] = isAdmin();
                List<Survey> surveys = _context.Surveys.Where(s => s.OwnerId == userId).ToList();
                ViewData["UserSurveys"] = surveys;
                ViewData["AllSurveys"] = _context.Surveys.ToList();
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
