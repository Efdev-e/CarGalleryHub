using CarGalleryHub.Application.DTOs.User.Other;
using CarGalleryHub.MVC.Models.DTOs.User;
using CarGalleryHub.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarGalleryHub.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly ApiClient _apiclient;

        public UsersController(ApiClient apiclient) => _apiclient = apiclient;

        public bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") != "Admin";
        }

        public async Task<IActionResult> Index()
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            var response = await _apiclient.GetAsync<List<UserInfoDto>>("api/User/admin/list");
            if (response is null)
            {
                TempData["ErrorMessage"] = "Yanıt Gelmedi";
                return View(new UserPageViewDto());
            }

            if (!response.Success || response.Data is null)
            {
                TempData["ErrorMessage"] = response.Message;
                return View(new UserPageViewDto());
            }

            return View(new UserPageViewDto() { Users = response.Data });
        }
    }
}
