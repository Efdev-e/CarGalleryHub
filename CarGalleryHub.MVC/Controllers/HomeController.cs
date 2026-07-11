using CarGalleryHub.Application.DTOs.Advert;
using CarGalleryHub.Application.DTOs.CartItem;
using CarGalleryHub.Domain.Enum;
using CarGalleryHub.MVC.Models;
using CarGalleryHub.MVC.Models.DTOs.Advert;
using CarGalleryHub.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CarGalleryHub.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiClient _apiclient;

        public HomeController(ApiClient apiclient)
        {
            _apiclient = apiclient;
        }

        public IActionResult Index()
        {
            return View();
        }

        private bool HasExistingToken()
        {
            return !string.IsNullOrWhiteSpace(HttpContext.Session.GetString("JwtToken"));
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddToCart(int advertId, int quantity = 1)
        {
            if (!HasExistingToken())
                return RedirectToAction("Login", "Account");

            if (advertId <= 0 || quantity <= 0)
            {
                TempData["errorMessage"] = "Geçersiz ilan veya miktar.";
                return RedirectToAction(nameof(Adverts));
            }

            var response = await _apiclient.PostAsync<bool>("api/Cart/addItem", new CreateCartItemDto
            {
                AdvertId = advertId,
                Quantity = quantity
            });

            if (response is null || !response.Success)
            {
                TempData["errorMessage"] = response?.Message ?? "İlan sepete eklenemedi.";
                return RedirectToAction(nameof(Adverts));
            }

            TempData["successMessage"] = "İlan sepete eklendi.";
            return RedirectToAction(nameof(Adverts));
        }

        [HttpGet]
        public async Task<IActionResult> Adverts(int? page, string? name, [FromQuery] CarAvailability[]? availability, [FromQuery] CarStatus[]? status, [FromQuery] ColorType[]? color, [FromQuery] CategoryType? categoryType)
        {
            if (!page.HasValue)
                page = 1;

            var query = $"/api/Advert/GetAdvertsByPage/{page}?name={Uri.EscapeDataString(name ?? string.Empty)}";
            if (availability?.Length > 0)
                query += "&" + string.Join("&", availability.Select(a => $"availability={Uri.EscapeDataString(a.ToString())}"));
            if (status?.Length > 0)
                query += "&" + string.Join("&", status.Select(s => $"status={Uri.EscapeDataString(s.ToString())}"));
            if (color?.Length > 0)
                query += "&" + string.Join("&", color.Select(c => $"color={Uri.EscapeDataString(c.ToString())}"));
            if (categoryType.HasValue)
                query += $"&categoryType={Uri.EscapeDataString(categoryType.Value.ToString())}";

            var response = await _apiclient.GetAsync<List<AdvertView>>(query);
            if (response is null || !response.Success)
            {
                TempData["errorMessage"] = response?.Message ?? "İlanlar yüklenemedi.";
                return View(new AdvertPageViewDto { page = page ?? 1, Dtos = new List<AdvertView>() });
            }

            return View(new AdvertPageViewDto { page = page ?? 1, Dtos = response.Data ?? new List<AdvertView>() });
        }

        [HttpGet]
        public async Task<IActionResult> AdvertDetail(int id)
        {
            var response = await _apiclient.GetAsync<AdvertDto>($"api/Advert/{id}");
            if (response is null || !response.Success || response.Data is null)
            {
                TempData["errorMessage"] = response?.Message ?? "İlan detayı yüklenemedi.";
                return RedirectToAction(nameof(Adverts));
            }

            return View(response.Data);
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
