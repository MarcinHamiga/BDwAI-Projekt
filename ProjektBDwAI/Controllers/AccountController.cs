using Microsoft.AspNetCore.Mvc;
using ProjektBDwAI.Models;
using BC = BCrypt.Net.BCrypt;

namespace ProjektBDwAI.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }
        
        private string HashPassword(string Password)
        {
            return BC.HashPassword(Password);
        }

        private bool VerifyPassword(string providedPassword, string hashedPassword)
        {
            return BC.Verify(providedPassword, hashedPassword);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Username == model.Username))
                {
                    var existingUser = _context.Users.FirstOrDefault(u => u.Username == model.Username);

                    if (existingUser != null && VerifyPassword(model.Password, existingUser.Password)) {
                        HttpContext.Session.SetInt32("UserId", existingUser.Id);
                        HttpContext.Session.SetString("Username", existingUser.Username);

                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            if (_context.Users.Any(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Ta nazwa użytkownika jest już zajęta");
                return View();
            }
            
            model.Password = HashPassword(model.Password);

            _context.Users.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }
    }
}
