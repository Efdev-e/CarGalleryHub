using Azure;
using CarGalleryHub.Application.DTOs.Brand;
using CarGalleryHub.Application.DTOs.Car;
using CarGalleryHub.Application.DTOs.CarModel;
using CarGalleryHub.MVC.Models.DTOs.Brand;
using CarGalleryHub.MVC.Models.DTOs.CarModel;
using CarGalleryHub.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace CarGalleryHub.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CarModelsController : Controller
    {

        private readonly ApiClient _apiclient;
        public CarModelsController(ApiClient apiclient) => _apiclient = apiclient;
        #region main
        public bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") != "Admin";
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page, [FromQuery] string? name)
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            var query = $"api/CarModel/GetAllModels";

            if (page.HasValue)
                query += $"/{page}";
            else
            {
                query += $"/1";
            }
            if (!string.IsNullOrEmpty(name))
                query += $"?name={Uri.EscapeDataString(name)}";

            var response = await _apiclient.GetAsync<List<CarModelPageData>>(query);

            if (response is null)
            {
                ModelState.AddModelError(string.Empty, "Yanıt boş");
                return View(new CarModelPageView() { page = page ?? 1, Dtos = response?.Data ?? new List<CarModelPageData>() });
            }
            if (!response.Success)
            {
                return View(new CarModelPageView() { page = page ?? 1, Dtos = response?.Data ?? new List<CarModelPageData>() });
            }



            return View(new CarModelPageView() { page = page ?? 1, Dtos = response?.Data ?? new List<CarModelPageData>() });
        }
        #endregion

        #region CarModel
        [HttpGet]
        public async Task<IActionResult> CarModel(int id) 
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            var response_one = await _apiclient.GetAsync<CarModelInspectDto>($"api/CarModel/Inspect/{id}");
            if (response_one is null)
            {
                ModelState.AddModelError(string.Empty, "Yanıt boş");
                return RedirectToAction("Index");
            }
            if (!response_one.Success || response_one.Data is null)
            {
                TempData["ErrorMessage"] = "Model Bulunamadı";
                return RedirectToAction("Index");
            }

            return View(new CarModelnspectView() { data = response_one.Data });
        }

        #endregion

        #region Create

        [HttpGet]
        public async Task<IActionResult> Create() 
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            var response = await _apiclient.GetAsync<List<BrandListDto>>("api/Brand/GetAllBrand");

            if (response is null)
            {
                ModelState.AddModelError(string.Empty, "Yanıt boş");
                return View(new CarModelCreateView() { Brands =  new List<BrandListDto>(), CarModelData = new CarModelDataCreate() });
            }
            if (!response.Success || response.Data is null)
            {
                TempData["ErrorMessage"] = "Markalar Listelenemedi";
                return View(new CarModelCreateView() { Brands = response?.Data ?? new List<BrandListDto>(), CarModelData = new CarModelDataCreate() });
            }



            return View(new CarModelCreateView() { Brands = response?.Data ?? new List<BrandListDto>(), CarModelData = new CarModelDataCreate() });        
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind( Prefix = "CarModelData")]CarModelDataCreate carModelDataCreate)  
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            if (!ModelState.IsValid) 
            {
                return RedirectToAction("Create");
            }

            var response = await _apiclient.PostAsync<bool>("api/CarModel/create", new CarModelDto() 
            {
                BrandId = carModelDataCreate.BrandId,
                Model = carModelDataCreate.Model,
                ReleaseDate = carModelDataCreate.ReleaseDate,
                Series = carModelDataCreate.Series
            });

            if (response is null)
            {
                ModelState.AddModelError(string.Empty, "Yanıt boş");
                return RedirectToAction("Create");
            }
            if (!response.Success )
            {
                TempData["ErrorMessage"] = "Model Oluşturulamadı";
                return RedirectToAction("Create");
            }

            TempData["SuccessMessage"] = "Araba Modeli Oluşturuldu";
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int id) 
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            var response = await _apiclient.DeleteAsync<bool>($"api/CarModel/delete/{id}");

            if (response is null)
            {
                TempData["SuccessMessage"] = "Yanıt Boş";
                return RedirectToAction("Index");
            }
            if (!response.Success)
            {
                TempData["ErrorMessage"] = "Model Silinemedi";
                return RedirectToAction("Index");
            }

            TempData["SuccessMessage"] = "Model Silindi";
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id, CarModelDataCreate? view)
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });


            var listing = new CarModelDataCreate();
            if (view is null) 
            {
                var list = await _apiclient.GetAsync<CarModelDto>($"api/CarModel/{id}");
                if (list is null)
                {
                    ModelState.AddModelError(string.Empty, "Boş Yanıt");
                    return RedirectToAction("Index");
                }
                if (!list.Success || list.Data is null)
                {
                    TempData["ErrorMessage"] = "Model bulunamadı";
                    return RedirectToAction("Index");
                }

                listing = new CarModelDataCreate()
                {
                    Model = list.Data.Model,
                    BrandId = list.Data.BrandId,
                    ReleaseDate = list.Data.ReleaseDate,
                    Series = list.Data.Series,
                };

               
            }

            var response = await _apiclient.GetAsync<List<BrandListDto>>("api/Brand/GetAllBrand");

            if (response is null)
            {
                TempData["ErrorMessage"] = "Boş Yanıt";
                return RedirectToAction("Index");
            }
            if (!response.Success || response.Data is null)
            {
                TempData["ErrorMessage"] = "Markalar Listelenemedi";
                return RedirectToAction("Index");
            }

            return View(new CarModelUpdateView()
            {
                Brands = response.Data,
                CarModelId = id,
                CarModelData = view is null ? listing : view
            });
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CarModelUpdateView view)
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });



            if (!ModelState.IsValid)
                return View(view);

            var list = await _apiclient.PostAsync<bool>($"api/CarModel/update/{view.CarModelId}", new CarModelDto() 
            {
                BrandId = view.CarModelData.BrandId,
                Model = view.CarModelData.Model,
                ReleaseDate = view.CarModelData.ReleaseDate,
                Id = view.CarModelId,
                Series = view.CarModelData.Series
            });
            if (list is null)
            {
                ModelState.AddModelError(string.Empty, "Boş Yanıt");
                return View(view);

            }
            if (!list.Success || list.Data is false)
            {
                TempData["ErrorMessage"] = "Model Güncellenemedi";
                return View(view);
            }

            TempData["SuccessMessage"] = "Model Güncellendi";
            return RedirectToAction("Index");
        }

        #endregion

        #region AddCar

        [HttpGet]
        public async Task<IActionResult> AddCar(int id, CarInfoDto? model) 
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            var list = await _apiclient.GetAsync<CarModelDto>($"api/CarModel/{id}");
            if (list is null)
            {
                ModelState.AddModelError(string.Empty, "Boş Yanıt");
                return RedirectToAction("Index");
            }
            if (!list.Success || list.Data is null)
            {
                TempData["ErrorMessage"] = "Model bulunamadı";
                return RedirectToAction("Index");
            }

            var name = $"{list.Data.Model} {list.Data.Series}";

            var carlist = await _apiclient.GetAsync<List<CarInfoDto>>($"api/Car/GetAllCarms");

            if (carlist is null)
            {
                ModelState.AddModelError(string.Empty, "Boş Yanıt");
                return RedirectToAction("Index");
            }
            if (!carlist.Success || carlist.Data is null)
            {
                TempData["ErrorMessage"] = "Arabalar bulunamadı";
                return RedirectToAction("Index");
            }

            return View(new CarModelAddCarView() { carDataModel = carlist.Data, ModelName = name});
        }

        [HttpPost]
        public async Task<IActionResult> AddCar(int id, CarModelAddCarView view)
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            if (!ModelState.IsValid) 
            {
                TempData["ErrorMessage"] = "Veri Kabul edilmedi";
                return View(view.carDataModel);
            }

            var response = await _apiclient.PostNoBodyAsync<bool>($"api/CarModel/addCar/{view.CarId},{id}");

            if (response is null)
            {
                TempData["ErrorMessage"] = "Boş Yanıt";
                return View(view.carDataModel);
            }
            if (!response.Success || response.Data is false)
            {
                TempData["ErrorMessage"] = "Araba Eklenemedi";
                return View(view.carDataModel);
            }


            TempData["SuccessMessage"] = "Araba Eklendi.";
            return RedirectToAction("Index");
        }

        #endregion

    }
}
