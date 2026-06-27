using CarGalleryHub.Application.DTOs.Advert;
using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Domain.Enum;
using CarGalleryHub.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace CarGalleryHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly int SayfaBoyut = 10;
        public AdvertController(IUnitOfWork work)
        {
            unitOfWork = work;
        }



        [HttpGet("GetAdvertsByPage/{PageNumber}")]
        public async Task<IActionResult> GetAdvertsByPage(int PageNumber,[FromQuery] CarAvailability[]? availability, [FromQuery] ColorType[]? color, [FromQuery] CategoryType? categoryType, [FromQuery] CarStatus[]? status, [FromQuery] string? Name) 
        {
            var query = unitOfWork.Adverts.Query();
            query = query.Include(x => x.Car);

            if (availability is not null && availability.Length > 0)
            {
                query = query.Where(x => availability.Contains(x.Car.Availability));
            }

            if (color is not null && color.Length > 0)
            {
                query = query.Where(x => color.Contains(x.Car.Color));
            }

            if (status is not null && status.Length > 0)
            {
                query = query.Where(x => status.Contains(x.Car.Status));
            }

            if (categoryType.HasValue)
            {
                switch ((CategoryType) categoryType)
                {
                    case CategoryType.HighestPrice:
                        query = query.OrderByDescending(x => x.UnitPrice);
                        break;
                    case CategoryType.LowestPrice:
                        query = query.OrderBy(x => x.UnitPrice); 
                        break;
                    default:
                        query = query.OrderBy(x => x.Id);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(x => x.Id);
            }
            query = query.Where(x => x.IsDeleted == false);
            if (!string.IsNullOrEmpty(Name)) 
            {
                query = query.Where(x => EF.Functions.Like(x.AdvertTitle, $"%{Name}%"));
            }

            var adverts = await query
                .Skip((PageNumber - 1) * SayfaBoyut)
                .Take(SayfaBoyut)
                .ToListAsync();

            if (adverts is null)
                return Invalid("İlanlar Yok");

            var dto = adverts.Select(x => new AdvertView()
            {
                Id = x.Id,
                AdvertTitle = x.AdvertTitle,
                Description = x.Description,
                UnitPrice = x.UnitPrice,
                ImageUrl = x.Thumbnails != null && x.Thumbnails.Any()
               ? x.Thumbnails.FirstOrDefault()!.ImageUrl
               : ""
            }).ToList();

            return Ok(dto);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdvertById(int id) 
        {
            var advert = await unitOfWork.Adverts.GetByIdIncludedAsync(id, u => u.Thumbnails, u => u.Car);
            if (advert is null) return Invalid("Yok");
            if (advert.IsDeleted) return Invalid("Advert Null");
            var advertDto = new AdvertDto()
            {
                AdvertTitle = advert.AdvertTitle,
                CarId = advert.CarId,
                Description = advert.Description,
                SellerId = advert.SellerId,
                Id = advert.Id,
                Thumbnails = advert.Thumbnails.Select(x => new Application.DTOs.Image.ImageDto() { ImageUrl = x.ImageUrl, ImageType = x.ImageType}).ToList(),
                CreatedAt = advert.CreatedAt,
                UpdatedAt = advert.UpdatedAt,
                Warranty = advert.Warranty,
                UnitPrice = advert.UnitPrice,
                CarName = $"{advert.Car.BrandName} {advert.Car.Color} {advert.Car.ModelName}"
            };

            return Ok(advertDto);
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateAdvert([FromBody] AdvertDto advertDto) 
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

            return Ok(data: true);
        }

        [HttpPost("update/{advertId}")]
        [Authorize]
        public async Task<IActionResult> UpdateAdvert(int advertId, [FromBody] UpdateAdvertDto updateAdvert)
        {
            var advert = await unitOfWork.Adverts.GetByIdIncludedAsync(advertId, u => u.Thumbnails);
            
            if (advert is null) return Invalid("Advert Null");
            if (advert.IsDeleted) return Invalid("Advert Null");
            if (!IsAdmin()) 
            {
                if (advert.SellerId != GetUserId()) return Invalid("İlan sahibi değilsiniz");
            }
            if (updateAdvert is null) return Invalid("İlan bilgileri verilmedi");

            advert.AdvertTitle = updateAdvert?.AdvertTitle ?? advert.AdvertTitle;
            advert.UnitPrice = updateAdvert?.UnitPrice ?? advert.UnitPrice;
            advert.Thumbnails = updateAdvert?.Thumbnails?.Select(x => 
                                new Image() { ImageUrl = x.ImageUrl, ImageType = x.ImageType, AdvertId = advert.Id }).ToList() ?? advert.Thumbnails;
            

            unitOfWork.Adverts.Update(advert);
            await unitOfWork.SaveChangesAsync();

            return Ok(data: true);
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAdvertById(int id) 
        {
            var advert = await unitOfWork.Adverts.GetByIdAsync(id);
            if (advert is null) return Invalid();
            if (!IsAdmin())
            {
                if (advert.SellerId != GetUserId()) return Invalid(data: false);
            }
            advert.DeleteIt();
            unitOfWork.Adverts.Update(advert);
            await unitOfWork.SaveChangesAsync();

            return Ok(data: true);
        }

        

       
    }
}
