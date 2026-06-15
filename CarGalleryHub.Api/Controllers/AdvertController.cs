using CarGalleryHub.Application.DTOs.Advert;
using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace CarGalleryHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public AdvertController(IUnitOfWork work)
        {
            unitOfWork = work;
        }



        [HttpGet]
        public async Task<IActionResult> GetAdvertById(int id) 
        {
            var advert = await unitOfWork.Adverts.GetByIdAsync(id);
            if (advert is null) return Invalid("Yok");

            var advertDto = new AdvertDto()
            {
                AdvertTitle = advert.AdvertTitle,
                CarId = advert.CarId,
                Description = advert.Description,
                SellerId = advert.SellerId,
                Id = advert.Id,
                Thumbnails = advert.Thumbnails.Select(x => new Application.DTOs.Image.ImageDto() { ImageUrl = x.ImageUrl, ImageType = x.ImageType}).ToList(),
                CreatedAt = advert.CreatedAt,
                UpdatedAt = advert.UpdatedAt
            };

            return Ok(advert);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAdvert(AdvertDto advertDto) 
        {
            if (advertDto is null) return Invalid();

            var advert = new Advert()
            {
                AdvertTitle = advertDto.AdvertTitle,
                CarId = advertDto.CarId,
                Description = advertDto.Description,
                SellerId = GetUserId(),
                Thumbnails = advertDto.Thumbnails.Select(x => new Image() { ImageUrl = x.ImageUrl, ImageType = x.ImageType}).ToList(),
                UnitPrice = advertDto.UnitPrice,
                Warranty = advertDto.Warranty
            };

            await unitOfWork.Adverts.AddAsync(advert);
            await unitOfWork.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateAdvert(UpdateAdvertDto advertDto, int advertId)
        {
            var advert = await unitOfWork.Adverts.GetByIdAsync(advertId);
            if (advert is null) return Invalid();
            if (!IsAdmin()) 
            {
                if (advert.SellerId != GetUserId()) return Invalid();
            }
            if (advertDto is null) return Invalid();

            advert.AdvertTitle = advertDto?.AdvertTitle ?? advert.AdvertTitle;
            advert.UnitPrice = advertDto?.UnitPrice ?? advert.UnitPrice;
            advert.Thumbnails = advertDto?.Thumbnails?.Select(x => 
                                new Image() { ImageUrl = x.ImageUrl, ImageType = x.ImageType, AdvertId = advert.Id }).ToList() ?? advert.Thumbnails;
            

            await unitOfWork.Adverts.AddAsync(advert);
            await unitOfWork.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> DeleteAdvertById(int id) 
        {
            var advert = await unitOfWork.Adverts.GetByIdAsync(id);
            if (advert is null) return Invalid();
            if (!IsAdmin())
            {
                if (advert.SellerId != GetUserId()) return Invalid();
            }

            unitOfWork.Adverts.Remove(advert);
            await unitOfWork.SaveChangesAsync();

            return Ok();
        }

        

       
    }
}
