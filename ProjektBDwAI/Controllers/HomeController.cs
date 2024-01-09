using Microsoft.AspNetCore.Mvc;
using ProjektBDwAI.Models;
using System.Diagnostics;

namespace ProjektBDwAI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                ViewData["Username"] = HttpContext.Session.GetString("Username");
                ViewData["UserId"] = HttpContext.Session.GetInt32("UserId");
                return View();
            }

            return RedirectToAction("Index", "Account");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
