using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SneakerStore.Services.Dtos;
using SneakerStore.Services.Services;
using System.Reflection;
using System.Security.Claims;

namespace SneakerStore.Services.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            try
            {
                await _userService.RegisterAsync(request);

                TempData["Success"] = "Registration Successful. Please login";

                return RedirectToAction(nameof(Login));
            }

            catch(Exception)
            {
                ModelState.AddModelError("", "An unxpected error happened please try again later.");
                return View(request);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            try
            {
                var user = await _userService.LoginAsync(request);

                if(user == null)
                {
                    ModelState.AddModelError("", "Invalid email or password");
                    return View(request);
                }

                TempData["Success"] = "Login successful";

                var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.Role,user.Role.ToString())
                };

                var identity = new ClaimsIdentity(
                        claims,
                        CookieAuthenticationDefaults.AuthenticationScheme
                    );

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal
                    );


                return RedirectToAction("Index", "Profile");
            }

            catch(UnauthorizedAccessException)
            {
                ModelState.AddModelError("","Inavlid email or password");
                return View(request);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme
                );

            return RedirectToAction(nameof(Login));
        }
    }
}
