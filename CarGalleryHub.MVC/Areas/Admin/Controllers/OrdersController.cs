using CarGalleryHub.Application.DTOs.Order;
using CarGalleryHub.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarGalleryHub.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly ApiClient _apiclient;

        public OrdersController(ApiClient apiclient)
        {
            _apiclient = apiclient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Index", "Home", new { area = "" });

            // api/Order/GetAllOrders is a POST request
            var response = await _apiclient.PostAsync<List<OrderSimpleInfoDto>>("api/Order/GetAllOrders", new { });
            if (response is null)
            {
                TempData["errorMessage"] = "Yanıt alınamadı.";
                return View(new List<OrderSimpleInfoDto>());
            }

            if (!response.Success || response.Data is null)
            {
                TempData["errorMessage"] = response.Message ?? "Siparişler yüklenirken bir hata oluştu.";
                return View(new List<OrderSimpleInfoDto>());
            }

            return View(response.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
                return RedirectToAction("Index", "Home", new { area = "" });

            var response = await _apiclient.GetAsync<OrderInfoDto>($"api/Order/GetOrderByIdForAdmin/{id}");
            if (response is null)
            {
                TempData["errorMessage"] = "Yanıt alınamadı.";
                return RedirectToAction("Index");
            }

            if (!response.Success || response.Data is null)
            {
                TempData["errorMessage"] = response.Message ?? "Sipariş detayları yüklenirken bir hata oluştu.";
                return RedirectToAction("Index");
            }

            return View(response.Data);
        }
    }
}
