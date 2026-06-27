using CarGalleryHub.Application.DTOs.Advert;
using CarGalleryHub.Application.DTOs.Car;
using CarGalleryHub.Application.DTOs.Image;
using CarGalleryHub.Domain.Enum;
using CarGalleryHub.MVC.Models.DTOs.Advert;
using CarGalleryHub.MVC.Services;
using Iyzipay.Request.V2.Subscription;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Xml;

namespace CarGalleryHub.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdvertsController : Controller
    {
        private readonly ApiClient _apiclient;
        public AdvertsController(ApiClient client) => _apiclient = client;

        [HttpGet]
        [Route("admin/adverts")]
        public async Task<IActionResult> Index(int? page, string? name, [FromQuery] CarAvailability[]? availability, [FromQuery] CarStatus[]? status, [FromQuery] ColorType[]? color,[FromQuery] CategoryType? categoryType)
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });


            if (!page.HasValue)
                page = 1;
            var query = $"/api/Advert/GetAdvertsByPage/{page}?name={name}";
            if (availability?.Length > 0)
                query += "&" + string.Join("&", availability.Select(a => $"availability={a}"));
            if (status?.Length > 0)
                query += "&" + string.Join("&", status.Select(s => $"status={s}"));
            if (color?.Length > 0)
                query += "&" + string.Join("&", color.Select(c => $"color={c}"));
            if (categoryType.HasValue)
                query += $"&categoryType={categoryType}";


            var response = await _apiclient.GetAsync<List<AdvertView>>(query);
            if (response is null)
            {
                TempData["ErrorMessage"] = "Yanıt Gelmedi";
                return View(new AdvertPageViewDto() { page = page ?? 1, Dtos = response?.Data ?? new List<AdvertView>() });
            }
            if (!response.Success || response.Data is null) 
            {
                TempData["ErrorMessage"] = "Hata";
                return View(new AdvertPageViewDto() { page = page ?? 1, Dtos = response.Data ?? new List<AdvertView>() });
            }


            return View(new AdvertPageViewDto() { page = page ?? 1, Dtos = response.Data ?? new List<AdvertView>() });
        }

        // Get

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Advert(int id)
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });


            var list = await _apiclient.GetAsync<AdvertDto>($"api/Advert/{id}");
            if (list is null) { 
                ModelState.AddModelError(string.Empty,"Veri boş");
                return View(id);
            }
            if (!list.Success || list.Data is null)
            {
                ModelState.AddModelError(string.Empty, "İlan bulunamadı.");
                return View(id);
            }

            var newData = new AdvertView()
            {
                AdvertTitle = list.Data.AdvertTitle,
                Description = list.Data.Description,
                UnitPrice = list.Data.UnitPrice,
                FullName = list.Data.CarName,
                Id = list.Data.Id,
                ImageUrl = list.Data.Thumbnails?.FirstOrDefault()?.ImageUrl ?? ""
            };
            return View(model: newData);
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int id, AdvertViewDto? dto)
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            if (dto is not null) 
            {
                id = dto.Id;
            }

            var list = await _apiclient.GetAsync<AdvertDto>($"api/Advert/{id}");
            if (list is null)
            {
                ModelState.AddModelError(string.Empty, "Boş Yanıt");
                return View(id);
            }
            if (!list.Success || list.Data is null)
            {
                ModelState.AddModelError(string.Empty, "İlan bulunamadı.");
                return View(id);
            }

            var Map = new AdvertUpdateModel() 
            {
                Id = list.Data.Id,
                AdvertTitle = list.Data.AdvertTitle,
                Description = list.Data.Description,
                UnitPrice = list.Data.UnitPrice,
                CarId = list.Data.CarId,
                ImageUrl = list.Data.Thumbnails[0].ImageUrl
            };

            var carInfos = await _apiclient.GetAsync<List<CarInfoDto>>("api/Car/GetAllCarms");

            

            return View(new AdvertViewDto() { Id = id, updateAdvert = Map, carModels = carInfos?.Data ?? new List<CarInfoDto>() });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int id, AdvertUpdateModel updateAdvert)
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            if (!ModelState.IsValid)
                return View(new AdvertViewDto() { Id = id });

            var advertsDto = new UpdateAdvertDto()
            {
                AdvertTitle = updateAdvert.AdvertTitle,
                CarId = updateAdvert.CarId,
                UnitPrice = updateAdvert.UnitPrice,
                Thumbnails = new List<ImageDto>()
                    {
                        new ImageDto() { ImageUrl = updateAdvert?.ImageUrl?? "", ImageType = ImageType.Thumbnail}
                    },
                Description = updateAdvert?.Description ?? ""
            };
            var advertUpdate = await _apiclient.PostAsync<bool>($"/api/Advert/update/{id}",advertsDto);

            if (advertUpdate is null)
            {
                ModelState.AddModelError(string.Empty, "Cevap boş");
                return View(new AdvertViewDto() { Id = id, updateAdvert = updateAdvert! });
            }

            if (!advertUpdate.Success)
            {
                ModelState.AddModelError(string.Empty, $"İşlem Başarısız \n\n Mesaj: {advertUpdate.Message}");

                return View(new AdvertViewDto() { Id = id, updateAdvert = updateAdvert! });
            }

            TempData["SuccessMessage"] = "İlan Başarıyla Güncellendi!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("admin/adverts/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            var del = await _apiclient.DeleteAsync<bool>($"api/Advert/delete/{id}");
            if (del is null) 
            {
                ModelState.AddModelError(string.Empty,"Silinemedi");
                return View("Index");
            }
            if (!del.Success) 
            {
                ModelState.AddModelError(string.Empty, "Silinemedi");
                return View("Index");
            }

            TempData["SuccessMessage"] = "İlan Başarıyla Silindi!";
            return RedirectToAction("Index");
        }
        //

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create() 
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            var data = await Setup();

            return View(data);
        }

        private async Task<AdvertCreateViewDto> Setup(CreateAdvertDto? dto = null) 
        {

            var carInfos = await _apiclient.GetAsync<List<CarInfoDto>>("api/Car/GetAllCarms");
            var nes = new AdvertCreateViewDto() { Cars = carInfos?.Data ?? new List<CarInfoDto>() };
            if (dto is not null) 
            {
                nes.createAdvertDto = dto;
            }
            return nes;
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create([Bind(Prefix = "createAdvertDto")] CreateAdvertDto dto) 
        {
            if (IsAdmin())
                return RedirectToAction("Index", "Home", new { area = "" });

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
                return Redirect("/login");
            ModelState.Remove("createAdvertDto.SellerId");
            dto.SellerId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
            if (!ModelState.IsValid)
            {
                var data = await Setup(dto);
                return View(data);
            }

            var newAdvert = new AdvertDto()
            {
                AdvertTitle = dto.AdvertTitle,
                Description = dto.Description,
                CarId = dto.CarId,
                SellerId = dto.SellerId,
                UnitPrice = dto.UnitPrice,
                Warranty = dto.Warranty,
                Thumbnails = new List<Application.DTOs.Image.ImageDto>() 
                {
                    new Application.DTOs.Image.ImageDto() 
                    {
                        ImageType = Domain.Enum.ImageType.Thumbnail,
                        ImageUrl = dto.ImageUrl
                    }
                }                
            };
            var createAdvert = await _apiclient.PostAsync<bool>("api/Advert/create", newAdvert);
            if (createAdvert is null) 
            {
                ModelState.AddModelError(string.Empty,"Boş yanıt");
                var data = await Setup(dto);
                return View(data);
            }
            
            if (!createAdvert.Success) 
            {
                ModelState.AddModelError(string.Empty, "Başarısız İşlem");
                var data = await Setup(dto);
                return View(data);
            }

            TempData["SuccessMessage"] = "İlan Başarıyla Oluşturuldu!";
            return RedirectToAction("Index");       
        }

        public bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") != "Admin";
        }
    }
}
