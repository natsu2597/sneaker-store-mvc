using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SneakerStore.Services.Dtos;
using SneakerStore.Services.Models;
using SneakerStore.Services.Services;
using SneakerStore.Services.Settings;
using System.Reflection;
using System.Security.Claims;

namespace SneakerStore.Services.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPasswordResetService _passwordResetService;
        private readonly ApplicationSettings _applicationSettings;

        public AccountController(IUserService userService, IPasswordResetService passwordResetService, ApplicationSettings applicationSettings)
        {
            _userService = userService;
            _passwordResetService = passwordResetService;
            _applicationSettings = applicationSettings;
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

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            if(!ModelState.IsValid)
                return View(request);

            var baseUrl = _applicationSettings.PublicUrl;

            await _passwordResetService.PasswordResetRequestAsync(request.Email, baseUrl);

            TempData["Success"] = "If an account exists with that email address, " +
        "a password reset link has been sent.";

            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token)
        {
            if (!await _passwordResetService.ValidateTokenAsync(token))
                return View("Invalid reset token!");

            var model = new ResetPasswordRequest
            {
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var success = await _passwordResetService.ResetPasswordASync(request.Token, request.NewPassword);

            if(!success)
            {
                ModelState.AddModelError("","This password reset link is invalid or has expired");
                return View(request);
            }

            TempData["Success"] = "Password reset successfully";

            return RedirectToAction(nameof(Login));
        }

    }
}
