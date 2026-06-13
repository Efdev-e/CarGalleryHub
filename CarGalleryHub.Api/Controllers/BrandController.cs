using CarGalleryHub.Application.DTOs.Brand;
using CarGalleryHub.Application.DTOs.CarModel;
using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;

namespace CarGalleryHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public BrandController(IUnitOfWork work)
        {
            _unitOfWork = work;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandById(int id) 
        {
            var Brand = await _unitOfWork.Brands.GetByIdAsync(id);
            if (Brand is null || Brand.BrandName is null) return Invalid();
            var result = new BrandInfoDto() { BrandName = Brand.BrandName };
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateBrand(BrandInfoDto brandInfo) 
        {
            if (!IsAdmin()) return Invalid("Yetkisiz İşlem");
            if (brandInfo is null || brandInfo.BrandName is null) return Invalid("Brand ismi verilmedi");
            var newBrand = new Brand() 
            {
                BrandName = brandInfo.BrandName
            };

            await _unitOfWork.Brands.AddAsync(newBrand);
            await _unitOfWork.SaveChangesAsync();

            return Ok($"{brandInfo.BrandName} Oluşturuldu");
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> AddCarModelToBrand(CreateCarModelDto carModelDto, int brandId)
        {
            if (!IsAdmin()) return Invalid("Yetkisiz İşlem");
            if (carModelDto is null) return Invalid("Gerekli Parametreler Verilmedi");


            var Brand = await _unitOfWork.Brands.GetByIdAsync(brandId);
            if (Brand is null || Brand.BrandName is null) return Invalid();

            Brand.CarModels.Add(new CarModel() 
            {
                BrandId = brandId,
                Cars = new List<Car>(),
                Model = carModelDto.Model,
                Series = carModelDto.Series,
                ReleaseDate = carModelDto.ReleaseDate
            });

            _unitOfWork.Brands.Update(Brand);
            await _unitOfWork.SaveChangesAsync();

            return Ok("Eklendi");
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> RemoveCarModelFromBrand(int brandId, int CarModelId)
        {
            if (!IsAdmin()) return Invalid("Yetkisiz İşlem");

            var Msg = "";

            var Brand = await _unitOfWork.Brands.GetByIdAsync(brandId);
            if (Brand is null || Brand.BrandName is null) return Invalid();
            var CarModel = await _unitOfWork.CarModels.GetByIdAsync(CarModelId);
            if (CarModel is null || CarModel.Model is null) return Invalid();

            if (Brand.CarModels.Contains(CarModel)) { Brand.CarModels.Remove(CarModel); Msg = "CarModel is Removed"; await _unitOfWork.SaveChangesAsync(); }
            else Msg = "CarModel doesn't exist";

            return Ok(Msg);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> DeleteBrandById(int id)
        {
            var Brand = await _unitOfWork.Brands.GetByIdAsync(id);
            if (Brand is null || Brand.BrandName is null) return Invalid("Bulunamadı");

            _unitOfWork.Brands.Remove(Brand);
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBrand(BrandInfoDto brandInfo,int id)
        {
            var Brand = await _unitOfWork.Brands.GetByIdAsync(id);
            if (Brand is null) return Invalid();
            Brand.BrandName = brandInfo.BrandName;
            _unitOfWork.Brands.Update(Brand);
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }
    }
}
