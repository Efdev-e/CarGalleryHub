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
                TempData["errorMessage"] = "Yanıt Gelmedi";
                return View(new UserPageViewDto());
            }

            if (!response.Success || response.Data is null)
            {
                TempData["errorMessage"] = response.Message;
                return View(new UserPageViewDto());
            }

            return View(new UserPageViewDto() { Users = response.Data });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            var response = await _apiclient.GetAsync<List<UserInfoDto>>("api/User/admin/list");
            if (response == null || !response.Success || response.Data == null)
            {
                TempData["errorMessage"] = "Kullanıcı listesi alınamadı.";
                return RedirectToAction("Index");
            }

            var user = response.Data.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                TempData["errorMessage"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Index");
            }

            var currentUserId = HttpContext.Session.GetString("UserId");
            ViewBag.IsSelfEdit = user.Id.ToString() == currentUserId;

            var dto = new AdminUserUpdateDto
            {
                Id = user.Id,
                FirstName = user.FirstName ?? "",
                LastName = user.LastName ?? "",
                Email = user.Email ?? "",
                Role = user.Role
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminUserUpdateDto dto)
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            if (!ModelState.IsValid)
                return View(dto);

            var currentUserId = HttpContext.Session.GetString("UserId");
            var currentUserRole = HttpContext.Session.GetString("UserRole");

            if (dto.Id.ToString() == currentUserId && dto.Role.ToString() != currentUserRole)
            {
                TempData["errorMessage"] = "Kendi rütbeni düşüremezsin.";
                return View(dto);
            }

            var response = await _apiclient.PostAsync<bool>("api/User/admin/update-user", dto);
            if (response != null && response.Success && response.Data)
            {
                TempData["successMessage"] = "Kullanıcı başarıyla güncellendi.";
                return RedirectToAction("Index");
            }

            TempData["errorMessage"] = response?.Message ?? "Güncelleme başarısız oldu.";
            return View(dto);
        }
    }
}
