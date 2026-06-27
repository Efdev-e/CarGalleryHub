using CarGalleryHub.Application.DTOs.Brand;
using CarGalleryHub.Application.DTOs.CarModel;
using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Domain.Enum;
using CarGalleryHub.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;

namespace CarGalleryHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly int BrandPage = 10;

        public BrandController(IUnitOfWork work)
        {
            _unitOfWork = work;
        }

        [HttpGet("GetAllBrand/{page}")]
        public async Task<IActionResult> GetAllBrand(int page, [FromQuery] string? name) 
        {
            var query = _unitOfWork.Brands.Query();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(x => EF.Functions.Like(x.BrandName, $"%{name}%"));

            var brands = await query.OrderBy(x => x.BrandName)
                .Skip((page - 1) * BrandPage)
                .Take(BrandPage)
                .ToListAsync();

            if (brands is null || !brands.Any()) 
            {
                return Invalid("Brand bulunamadı.");
            }

            var list = brands.Select(x => new BrandListDto() 
            {
                Id = x.Id,
                BrandName = x.BrandName
            });

            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandById(int id) 
        {
            var Brand = await _unitOfWork.Brands.GetByIdAsync(id);
            if (Brand is null || Brand.BrandName is null) return Invalid();
            var result = new BrandInfoDto() { BrandName = Brand.BrandName };
            return Ok(result);
            
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateBrand([FromBody] BrandInfoDto brandInfo) 
        {
            if (!IsAdmin()) return Invalid("Yetkisiz İşlem");
            if (brandInfo is null || brandInfo.BrandName is null) return Invalid("Brand ismi verilmedi");
            var newBrand = new Brand() 
            {
                BrandName = brandInfo.BrandName
            };

            await _unitOfWork.Brands.AddAsync(newBrand);
            await _unitOfWork.SaveChangesAsync();

            return Ok(true);
        }

        [HttpPost("addModel/{carModelId},{brandId}")]
        [Authorize]
        public async Task<IActionResult> AddCarModelToBrand(int carModelId, int brandId)
        {
            if (!IsAdmin()) return Invalid("Yetkisiz İşlem");
            var carModel = await _unitOfWork.CarModels.GetByIdIncludedAsync(carModelId, u => u.Brand);
            if (carModel is null) return Invalid(false,"Car Model bulunamadı");

            var Brand = await _unitOfWork.Brands.GetByIdIncludedAsync(brandId, u => u.CarModels);
            if (Brand is null || Brand.BrandName is null) return Invalid(false);

            if (Brand.CarModels is null) 
            {
                Brand.CarModels = new List<CarModel>();
            }

            if (!Brand.CarModels.Contains(carModel)) 
            {
                Brand.CarModels.Add(carModel);
            }

         

            _unitOfWork.Brands.Update(Brand);
            await _unitOfWork.SaveChangesAsync();

            return Ok(true);
        }

        [HttpPost("removeModel/{brandId},{CarModelId}")]
        [Authorize]
        public async Task<IActionResult> RemoveCarModelFromBrand(int brandId, int CarModelId)
        {
            if (!IsAdmin()) return Invalid("Yetkisiz İşlem");

            var Msg = "";

            var Brand = await _unitOfWork.Brands.GetByIdIncludedAsync(brandId, u => u.CarModels);
            if (Brand is null || Brand.BrandName is null) return Invalid(false);
            var CarModel = await _unitOfWork.CarModels.GetByIdIncludedAsync(CarModelId, u => u.Brand);
            if (CarModel is null || CarModel.Model is null) return Invalid(false);

            if (Brand.CarModels.Contains(CarModel)) { 
                Msg = "CarModel is Removed";
                Brand.CarModels.Remove(CarModel);
                _unitOfWork.Brands.Update(Brand);
                await _unitOfWork.SaveChangesAsync(); 
            }
            else Msg = "CarModel doesn't exist";

            return Ok(true,Msg);
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBrandById(int id)
        {
            if (!IsAdmin()) return Invalid("Yetkisiz Erişim");
            var Brand = await _unitOfWork.Brands.GetByIdAsync(id);
            if (Brand is null || Brand.BrandName is null) return Invalid("Bulunamadı");

            _unitOfWork.Brands.Remove(Brand);
            await _unitOfWork.SaveChangesAsync();

            return Ok(true);
        }

        [HttpPost("update/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBrand(int id, [FromBody] BrandInfoDto brandInfo)
        {
            if (!IsAdmin()) return Invalid("Yetkisiz Erişim");
            var Brand = await _unitOfWork.Brands.GetByIdAsync(id);
            if (Brand is null) return Invalid();
            Brand.BrandName = brandInfo.BrandName;
            _unitOfWork.Brands.Update(Brand);
            await _unitOfWork.SaveChangesAsync();
            return Ok(true);
        }

        [HttpGet("GetBrandModels")]
        [Authorize]
        public async Task<IActionResult> GetBrandModels(int id)
        {
            var Brand = await _unitOfWork.Brands.GetByIdIncludedAsync(id, u => u.CarModels);
            if (Brand is null) return Invalid();

            var brands = Brand.CarModels.Select(x => new CarModelData() 
            {
                FullName = $"{x.Model} {x.Series}",
                id = x.Id
            }).ToList();
            return Ok(brands);
        }
    }
}
