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
                ModelState.AddModelError(string.Empty, "Yanit bos");
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
                ModelState.AddModelError(string.Empty, "Yanit bos");
                return RedirectToAction("Index");
            }
            if (!response_one.Success || response_one.Data is null)
            {
                TempData["errorMessage"] = "Model Bulunamadi";
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
                ModelState.AddModelError(string.Empty, "Yanit bos");
                return View(new CarModelCreateView() { Brands =  new List<BrandListDto>(), CarModelData = new CarModelDataCreate() });
            }
            if (!response.Success || response.Data is null)
            {
                TempData["errorMessage"] = "Markalar Listelenemedi";
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
                ModelState.AddModelError(string.Empty, "Yanit bos");
                return RedirectToAction("Create");
            }
            if (!response.Success )
            {
                TempData["errorMessage"] = "Model Olusturulamadi";
                return RedirectToAction("Create");
            }

            TempData["successMessage"] = "Araba Modeli Olusturuldu";
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
                TempData["successMessage"] = "Yanit Bos";
                return RedirectToAction("Index");
            }
            if (!response.Success)
            {
                TempData["errorMessage"] = "Model Silinemedi";
                return RedirectToAction("Index");
            }

            TempData["successMessage"] = "Model Silindi";
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
                    ModelState.AddModelError(string.Empty, "Bos Yanit");
                    return RedirectToAction("Index");
                }
                if (!list.Success || list.Data is null)
                {
                    TempData["errorMessage"] = "Model bulunamadi";
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
                TempData["errorMessage"] = "Bos Yanit";
                return RedirectToAction("Index");
            }
            if (!response.Success || response.Data is null)
            {
                TempData["errorMessage"] = "Markalar Listelenemedi";
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
                ModelState.AddModelError(string.Empty, "Bos Yanit");
                return View(view);

            }
            if (!list.Success || list.Data is false)
            {
                TempData["errorMessage"] = "Model Güncellenemedi";
                return View(view);
            }

            TempData["successMessage"] = "Model Güncellendi";
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
                ModelState.AddModelError(string.Empty, "Bos Yanit");
                return RedirectToAction("Index");
            }
            if (!list.Success || list.Data is null)
            {
                TempData["errorMessage"] = "Model bulunamadi";
                return RedirectToAction("Index");
            }

            var name = $"{list.Data.Model} {list.Data.Series}";

            var carlist = await _apiclient.GetAsync<List<CarInfoDto>>($"api/Car/GetAllCarms");

            if (carlist is null)
            {
                ModelState.AddModelError(string.Empty, "Bos Yanit");
                return RedirectToAction("Index");
            }
            if (!carlist.Success || carlist.Data is null)
            {
                TempData["errorMessage"] = "Arabalar bulunamadi";
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
                TempData["errorMessage"] = "Veri Kabul edilmedi";
                return View(view.carDataModel);
            }

            var response = await _apiclient.PostNoBodyAsync<bool>($"api/CarModel/addCar/{view.CarId},{id}");

            if (response is null)
            {
                TempData["errorMessage"] = "Bos Yanit";
                return View(view.carDataModel);
            }
            if (!response.Success || response.Data is false)
            {
                TempData["errorMessage"] = "Araba Eklenemedi";
                return View(view.carDataModel);
            }


            TempData["successMessage"] = "Araba Eklendi.";
            return RedirectToAction("Index");
        }

        #endregion

    }
}
