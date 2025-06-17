using System.Diagnostics;
using System.Security.Claims;
using CalendareGit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalendareGit.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
 
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                await HttpContext.SignOutAsync();
                return RedirectToAction("Login", "Account");
            }

            var model = new DashboardViewModel
            {
                UserId = user.Id,
                Username = user.Username,
                Role = user.Role,
            };

            return View(model);
        }

        [Authorize(Policy = "AdminOnly")]
        public IActionResult AdminPanel()
        {
            return View();
        }
    }
}
