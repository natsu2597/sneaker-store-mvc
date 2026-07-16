using Microsoft.AspNetCore.Mvc;
using SneakerStore.Services.Dtos;
using SneakerStore.Services.Services;
using System.Reflection;

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

            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
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
                return RedirectToAction("Index", "Home");
            }

            catch(Exception ex)
            {
                ModelState.AddModelError("",ex.Message);
                return View(request);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            return RedirectToAction(nameof(Login));
        }
    }
}
