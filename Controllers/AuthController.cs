using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace CalendareGit.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                // Установка сессии
                HttpContext.Session.SetString("User", user.Username);
                HttpContext.Session.SetString("UserRole", User.IsInRole("1") ? "1" : "2");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Неверные логин или пароль";
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("User");
            return RedirectToAction("Login");
        }
    }
}
