using CarGalleryHub.Application.DTOs.Advert;
using CarGalleryHub.Application.DTOs.Brand;
using CarGalleryHub.Application.DTOs.CarModel;
using CarGalleryHub.MVC.Models.DTOs.Brand;
using CarGalleryHub.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalleryHub.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandsController : Controller
    {
        private readonly ApiClient _apiclient;
        public BrandsController(ApiClient apiclient) => _apiclient = apiclient;
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

            var query = $"api/Brand/GetAllBrand";

            if (page.HasValue)
                query += $"/{page}";
            else {
                query += $"/1";
            }
            if (!string.IsNullOrEmpty(name))
                query += $"?name={Uri.EscapeDataString(name)}";

            var response = await _apiclient.GetAsync<List<BrandListDto>>(query);

            if (response is null) 
            {
                ModelState.AddModelError(string.Empty, "Yanıt boş");
                return View(new BrandViewDto() { page = page ?? 1, Dtos = response?.Data ?? new List<BrandListDto>() });
            }
            if (!response.Success) 
            {
                return View(new BrandViewDto() { page = page ?? 1, Dtos = response?.Data ?? new List<BrandListDto>() });
            }



            return View(new BrandViewDto() { page = page ?? 1, Dtos = response?.Data ?? new List<BrandListDto>() }); 
        }
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create() 
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            return View();
        }

        [HttpPost]
        [Route("admin/brands/create")]
        public async Task<IActionResult> Create(BrandInfoDto brandInfoDto) 
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            if (!ModelState.IsValid)
                return View(brandInfoDto);

            var request = await _apiclient.PostAsync<bool>("api/Brand/create",brandInfoDto);

            if (request is null) 
            {
                ModelState.AddModelError(string.Empty, "Yanıt boş");
                return View(brandInfoDto);
            }
            if (!request.Success || request.Data is false)
            {
                TempData["ErrorMessage"] = "Başarısız";
                return View(brandInfoDto);
            }

            TempData["SuccessMessage"] = "Marka Oluşturuldu";
            return RedirectToAction("Index");
        }
        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id) 
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });



            var list = await _apiclient.GetAsync<BrandInfoDto>($"api/Brand/{id}");
            if (list is null)
            {
                ModelState.AddModelError(string.Empty, "Boş Yanıt");
                return RedirectToAction("Index");
            }
            if (!list.Success || list.Data is null)
            {
                TempData["ErrorMessage"] = "Marka bulunamadı";
                return RedirectToAction("Index");
            }

            var listing = new BrandListDto()
            {
                Id = id,
                BrandName = list.Data.BrandName
            };

            return View(listing);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, BrandListDto dto) 
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });



            if (!ModelState.IsValid)
                return View(dto);

            var list = await _apiclient.PostAsync<bool>($"api/Brand/update/{id}",new BrandInfoDto() { BrandName = dto.BrandName});
            if (list is null)
            {
                ModelState.AddModelError(string.Empty, "Boş Yanıt");
                return View(dto);
            }
            if (!list.Success || list.Data is false)
            {
                TempData["ErrorMessage"] = "Marka Güncellenemedi";
                return View(dto);
            }

            TempData["SuccessMessage"] = "Marka Güncellendi";
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        [HttpGet]
        [Route("admin/Brands/delete/{id:int}")]
        public async Task<IActionResult> Delete(int id) 
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            var list = await _apiclient.DeleteAsync<bool>($"api/Brand/delete/{id}");
            if (list is null)
            {
                ModelState.AddModelError(string.Empty, "Boş Yanıt");
                return RedirectToAction("Index");
            }
            if (!list.Success || list.Data is false)
            {
                TempData["ErrorMessage"] = "Marka Silinemedi";
                return RedirectToAction("Index");
            }
            TempData["SuccessMessage"] = "marka silindi";
            return RedirectToAction("Index");
        }
        #endregion

        #region Brand
        [HttpGet]
        [Route("admin/Brands/Brand/{id:int}")]
        public async Task<IActionResult> Brand(int id) 
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            var list = await _apiclient.GetAsync<BrandInfoDto>($"api/Brand/{id}");
            if (list is null)
            {
                ModelState.AddModelError(string.Empty, "Boş Yanıt");
                return RedirectToAction("Index");
            }
            if (!list.Success || list.Data is null)
            {
                TempData["ErrorMessage"] = "Marka bulunamadı";
                return RedirectToAction("Index");
            }

            var listing = new BrandListDto()
            {
                Id = id,
                BrandName = list.Data.BrandName
            };

            var brandlist = await _apiclient.GetAsync<List<CarModelData>>($"api/CarModel/GetAllmodels/{id}");

            if (brandlist is null)
            {
                ModelState.AddModelError(string.Empty, "Boş Yanıt");
                return RedirectToAction("Index");
            }
            if (!brandlist.Success || brandlist.Data is null)
            {
                TempData["ErrorMessage"] = "Modeller bulunamadı";
                return RedirectToAction("Index");
            }

            var dta = brandlist.Data.Select(x => new BasicCarModelData()
            {
                FullName = x.FullName,
                id = x.id
            }).ToList();

            return View(new BrandCarModelAdd() { id = id, BrandName = list.Data.BrandName, Dtos = dta });

        }
        #endregion

        #region AddModel
        [HttpGet]
        [Route("admin/Brand/AddModel/{id:int}")]
        public async Task<IActionResult> AddModel(int id) 
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            var list = await _apiclient.GetAsync<BrandInfoDto>($"api/Brand/{id}");
            if (list is null)
            {
                ModelState.AddModelError(string.Empty, "Boş Yanıt");
                return RedirectToAction("Index");
            }
            if (!list.Success || list.Data is null)
            {
                TempData["ErrorMessage"] = "Marka bulunamadı";
                return RedirectToAction("Index");
            }

            var brandlist = await _apiclient.GetAsync<List<CarModelData>>($"api/CarModel/GetAllmodels/{0}");

            if (brandlist is null)
            {
                ModelState.AddModelError(string.Empty, "Boş Yanıt");
                return RedirectToAction("Index");
            }
            if (!brandlist.Success || brandlist.Data is null)
            {
                TempData["ErrorMessage"] = "Modeller bulunamadı";
                return RedirectToAction("Index");
            }

            var dta = brandlist.Data.Select(x => new BasicCarModelData() 
            {
                FullName = x.FullName,
                id = x.id
            }).ToList();

            return View(new BrandCarModelAdd() { id = id, BrandName = list.Data.BrandName, Dtos = dta});
        }

        [HttpPost]
        [Route("admin/Brand/AddModel/{id:int}")]
        public async Task<IActionResult> AddModel(int id,[FromForm] int carModelId)
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });



            var list = await _apiclient.PostNoBodyAsync<bool>($"api/Brand/addModel/{carModelId},{id}");
            if (list is null)
            {
                ModelState.AddModelError(string.Empty, "Boş Yanıt");
                return RedirectToAction("Index");
            }
            if (!list.Success || list.Data is false)
            {
                TempData["ErrorMessage"] = "Marka bulunamadı";
                return RedirectToAction("Index");
            }
            TempData["SuccessMessage"] = "Model Eklendi";
            return RedirectToAction("Index");
        }
        #endregion

        #region RemoveModel
        [HttpGet]
        [Route("admin/Brand/RemoveModel/{id:int}")]
        public async Task<IActionResult> RemoveModel(int id)
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            var list = await _apiclient.GetAsync<BrandInfoDto>($"api/Brand/{id}");
            if (list is null)
            {
                ModelState.AddModelError(string.Empty, "Boş Yanıt");
                return RedirectToAction("Index");
            }
            if (!list.Success || list.Data is null)
            {
                TempData["ErrorMessage"] = "Marka bulunamadı";
                return RedirectToAction("Index");
            }

            var brandlist = await _apiclient.GetAsync<List<CarModelData>>($"api/CarModel/GetAllmodels/{id}");

            if (brandlist is null)
            {
                ModelState.AddModelError(string.Empty, "Boş Yanıt");
                return RedirectToAction("Index");
            }
            if (!brandlist.Success || brandlist.Data is null)
            {
                TempData["ErrorMessage"] = "Modeller bulunamadı";
                return RedirectToAction("Index");
            }

            var dta = brandlist.Data.Select(x => new BasicCarModelData()
            {
                FullName = x.FullName,
                id = x.id
            }).ToList();

            return View(new BrandCarModelView() { id = id, BrandName = list.Data.BrandName, Dtos = dta });
        }

        [HttpPost]
        [Route("admin/Brand/RemoveModel/{id:int}")]
        public async Task<IActionResult> RemoveModel(int id, [FromForm] int carModelId)
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });



            var list = await _apiclient.PostNoBodyAsync<bool>($"api/Brand/removeModel/{id},{carModelId}");
            if (list is null)
            {
                ModelState.AddModelError(string.Empty, "Boş Yanıt");
                return RedirectToAction("Index");
            }
            if (!list.Success || list.Data is false)
            {
                TempData["ErrorMessage"] = "Marka bulunamadı";
                return RedirectToAction("Index");
            }

            TempData["SuccessMessage"] = "Model Silindi";
            return RedirectToAction("Index");
        }
        #endregion
    }
}
