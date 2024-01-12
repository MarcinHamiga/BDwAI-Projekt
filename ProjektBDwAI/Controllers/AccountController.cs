using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
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

        public IActionResult Login()
        {
            var adminUser = _context.Users.FirstOrDefault(u => u.Username == "admin");
            if (adminUser == null)
            {
                adminUser = new User
                {
                    Username = "admin",
                    Password = HashPassword("Haslo_123"),
                    isAdmin = true
                };
                _context.Add(adminUser);
                _context.SaveChanges();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User model)
        {
            int isAdmin;
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Username == model.Username))
                {
                    var existingUser = _context.Users.FirstOrDefault(u => u.Username == model.Username);

                    if (existingUser.isAdmin)
                    {
                        isAdmin = 1;
                    }
                    else
                    {
                        isAdmin = 0;
                    }

                    if (existingUser != null && VerifyPassword(model.Password, existingUser.Password))
                    {
                        HttpContext.Session.SetInt32("UserId", existingUser.Id);
                        HttpContext.Session.SetString("Username", existingUser.Username);
                        HttpContext.Session.SetInt32("isAdmin", isAdmin);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Błędne hasło. Spróbuj ponownie.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Podany użytkownik nie istnieje. Spróbuj ponownie.");
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
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Change()
        {
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return View();
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Change(ChangeUserDataViewModel model)
        {
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                if (ModelState.IsValid)
                {
                    int userId = HttpContext.Session.GetInt32("UserId").Value;
                    bool isAdmin = HttpContext.Session.GetInt32("isAdmin").Value == 1;

                    if (model.Password == model.RetypePassword)
                    {
                        model.Password = HashPassword(model.Password);
                        var user = _context.Users.Find(userId);

                        if (user != null && model.Password != user.Password)
                        {
                            user.Password = model.Password;
                            _context.SaveChanges();
                            ViewData["Success"] = true;
                            return View();  // Return the view after a successful password change
                        }
                        ModelState.AddModelError(string.Empty, "Nowe hasło i stare hasło są identycznie. Spróbuj ponownie");
                    }

                    ModelState.AddModelError(string.Empty, "Hasła się nie zgadzają. Spróbuj ponownie");
                }

                return View();  // Return the view when ModelState is invalid
            }

            return RedirectToAction("Login");
        }

    }
}
