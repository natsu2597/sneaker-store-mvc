using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SneakerStore.Services.Dtos;
using SneakerStore.Services.Services;
using System.Security.Claims;

namespace SneakerStore.Services.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var user = await _userService.GetByIdAsync(userId);

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var user = await _userService.GetByIdAsync(userId);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateProfileRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            request.Id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _userService.UpdateProfileAsync(request);

            TempData["Success"] = "Profile updated successfully";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePassword request)
        {
            if (!ModelState.IsValid)
                return View(request);

            request.Id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _userService.ChangePasswordAsync(request);

            TempData["Success"] = "Password changed successfully";

            return RedirectToAction(nameof(Index));
        }
    }
}
