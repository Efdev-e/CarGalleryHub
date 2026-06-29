using CarGalleryHub.Application.DTOs.Car;
using CarGalleryHub.Application.DTOs.CarModel;
using CarGalleryHub.Domain.Enum;
using CarGalleryHub.MVC.Models.DTOs.Car;
using CarGalleryHub.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarGalleryHub.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CarsController : Controller
    {
        private readonly ApiClient _apiclient;

        public CarsController(ApiClient apiclient) => _apiclient = apiclient;

        public bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") != "Admin";
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page, [FromQuery] string? Name, [FromQuery] CarAvailability[]? availability, [FromQuery] ColorType[]? color, [FromQuery] CarStatus[]? status)
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            var currentPage = page ?? 1;
            var query = $"api/Car/GetAllCarms/{currentPage}";
            var parameters = new List<string>();

            if (!string.IsNullOrWhiteSpace(Name))
                parameters.Add($"Name={Uri.EscapeDataString(Name)}");

            if (availability?.Length > 0)
                parameters.AddRange(availability.Select(x => $"availability={x}"));

            if (color?.Length > 0)
                parameters.AddRange(color.Select(x => $"color={x}"));

            if (status?.Length > 0)
                parameters.AddRange(status.Select(x => $"status={x}"));

            if (parameters.Any())
                query += "?" + string.Join("&", parameters);

            var response = await _apiclient.GetAsync<List<CarInfoDto>>(query);
            if (response is null)
            {
                TempData["ErrorMessage"] = "Yanıt Gelmedi";
                return View(new CarPageViewDto() { page = currentPage });
            }

            if (!response.Success || response.Data is null)
            {
                TempData["ErrorMessage"] = response.Message;
                return View(new CarPageViewDto() { page = currentPage });
            }

            return View(new CarPageViewDto() { page = currentPage, Dtos = response.Data });
        }

        [HttpGet]
        public async Task<IActionResult> Car(int id)
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            var response = await _apiclient.GetAsync<CarDto>($"api/Car/{id}");
            if (response is null || !response.Success || response.Data is null)
            {
                TempData["ErrorMessage"] = "Araba bulunamadı";
                return RedirectToAction("Index");
            }

            return View(response.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            return View(await SetupForm());
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind(Prefix = "Car")] CarFormData car)
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            if (!ModelState.IsValid)
                return View(await SetupForm(car));

            var model = await _apiclient.GetAsync<CarModelInspectDto>($"api/CarModel/Inspect/{car.CarModelId}");
            if (model is null || !model.Success || model.Data is null)
            {
                TempData["ErrorMessage"] = "Model bilgisi alınamadı";
                return View(await SetupForm(car));
            }

            var dto = new CarDto()
            {
                BrandName = model.Data.BrandName,
                ModelName = model.Data.carModel.Model,
                Series = model.Data.carModel.Series,
                CarModelId = car.CarModelId,
                Year = car.Year,
                KM = car.KM,
                MotorPower = car.MotorPower,
                Color = car.Color,
                Status = car.Status,
                Availability = car.Availability
            };

            var response = await _apiclient.PostAsync<bool>("api/Car/create", dto);
            if (response is null || !response.Success)
            {
                TempData["ErrorMessage"] = response?.Message ?? "Araba oluşturulamadı";
                return View(await SetupForm(car));
            }

            TempData["SuccessMessage"] = "Araba oluşturuldu";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            var response = await _apiclient.GetAsync<CarDto>($"api/Car/{id}");
            if (response is null || !response.Success || response.Data is null)
            {
                TempData["ErrorMessage"] = "Araba bulunamadı";
                return RedirectToAction("Index");
            }

            var form = new CarFormData()
            {
                CarModelId = response.Data.CarModelId,
                Availability = response.Data.Availability,
                Color = response.Data.Color,
                KM = response.Data.KM,
                MotorPower = response.Data.MotorPower,
                Status = response.Data.Status,
                Year = response.Data.Year
            };

            return View(await SetupForm(form, id));
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, [Bind(Prefix = "Car")] CarFormData car)
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            if (!ModelState.IsValid)
                return View(await SetupForm(car, id));

            var dto = new UpdateCarDto()
            {
                Availability = car.Availability,
                CarModelId = car.CarModelId,
                Color = car.Color,
                KM = car.KM,
                MotorPower = car.MotorPower,
                Status = car.Status,
                Year = car.Year
            };

            var response = await _apiclient.PostAsync<bool>($"api/Car/update/{id}", dto);
            if (response is null || !response.Success)
            {
                TempData["ErrorMessage"] = response?.Message ?? "Araba güncellenemedi";
                return View(await SetupForm(car, id));
            }

            TempData["SuccessMessage"] = "Araba güncellendi";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            var response = await _apiclient.DeleteAsync<bool>($"api/Car/delete/{id}");
            if (response is null || !response.Success)
            {
                TempData["ErrorMessage"] = response?.Message ?? "Araba silinemedi";
                return RedirectToAction("Index");
            }

            TempData["SuccessMessage"] = "Araba silindi";
            return RedirectToAction("Index");
        }

        private async Task<CarFormViewDto> SetupForm(CarFormData? car = null, int id = 0)
        {
            var response = await _apiclient.GetAsync<List<CarModelPageData>>("api/CarModel/GetAllModels/1");
            return new CarFormViewDto()
            {
                Id = id,
                Car = car ?? new CarFormData(),
                Models = response?.Data ?? new List<CarModelPageData>()
            };
        }
    }
}
