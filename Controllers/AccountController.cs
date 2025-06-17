// Controllers/AccountController.cs
using CalendareGit.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class AccountController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<AccountController> _logger;

    public AccountController(AppDbContext context, ILogger<AccountController> logger)
    {
        _context = context;  
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userService.Authenticate(model.Username, model.Password);

            if (user != null)
            {
                // Создаем список claims
                var claims = new List<Claim>
            {
                // Обязательные claims
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Главный claim с ID
                new Claim(ClaimTypes.Name, user.Username), // Имя пользователя
                new Claim(ClaimTypes.Role, user.Role), // Роль пользователя
                
                // Дополнительные claims (по необходимости)
                new Claim("FullName", user.FullName ?? string.Empty),
                new Claim("Email", user.Email),
                new Claim("LastLogin", DateTime.UtcNow.ToString("o"))
            };

                // Создаем identity
                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                // Настройки аутентификации
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(model.RememberMe ?
                        TimeSpan.FromDays(30) : TimeSpan.FromMinutes(30))
                };

                // Вход в систему
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToLocal(returnUrl);
            }
        }

        // Обработка ошибки
        ModelState.AddModelError("", "Неверное имя пользователя или пароль");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        _logger.LogInformation("User logged out.");
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}