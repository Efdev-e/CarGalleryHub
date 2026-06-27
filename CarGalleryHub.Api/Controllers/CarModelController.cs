using CarGalleryHub.Application.DTOs.Car;
using CarGalleryHub.Application.DTOs.CarModel;
using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace CarGalleryHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarModelController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public CarModelController(IUnitOfWork work)
        {
            unitOfWork = work;
        }

        [HttpGet("GetAllmodels/{id:int}")]
        public async Task<IActionResult> GetAllmodels(int id) 
        {
            var CarModel = await unitOfWork.CarModels.GetAllAsync();
            if (CarModel is null) return Invalid("Model Yok");
            var query = CarModel.AsQueryable();
            
            if (id != 0) 
            {
                query = query.Where(x => x.BrandId == id);
            }

            var dto = query.Where(x => x.IsDeleted == false).Select(x => new CarModelData()
            {
                FullName = $"{x.Model} {x.Series}",
                id = x.Id
            }).ToList();

            return Ok(dto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarModelById(int id) 
        {
            var CarModel = await unitOfWork.CarModels.GetByIdAsync(id);
            if (CarModel is null) return Invalid("Model Yok");
            var dto = new CarModelDto() 
            {
                BrandId = CarModel.BrandId,
                Id = CarModel.Id,
                Model = CarModel.Model,
                Series = CarModel.Series,
                ReleaseDate = CarModel.ReleaseDate
            };
            return Ok(dto);
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateCarModel([FromBody] CarModelDto carModelDto)
        {
            if (!IsAdmin()) return Invalid("Yetkisiz Erişim");
            if (carModelDto is null || carModelDto.Series is null || carModelDto.Model is null) return Invalid("Parametreler Eksik");
            var doesBrandExist = await unitOfWork.Brands.GetByIdAsync(carModelDto.BrandId);
            if (doesBrandExist is null) return Invalid("Brand Yok");

            var carModel = new CarModel()
            {
                BrandId = carModelDto.BrandId,
                Model = carModelDto.Model,
                Series = carModelDto.Series,
                ReleaseDate = carModelDto.ReleaseDate,
                Cars = new List<Car>()
            };

            await unitOfWork.CarModels.AddAsync(carModel);
            await unitOfWork.SaveChangesAsync();

            return Ok("Oluşturuldu");
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCarModel(int id)
        {
            if (!IsAdmin()) return Invalid("Yetkisiz Erişim");
            var carModel = await unitOfWork.CarModels.GetByIdAsync(id);
            if (carModel is null) return Invalid("Model yok");

            unitOfWork.CarModels.Remove(carModel);
            await unitOfWork.SaveChangesAsync();

            return Ok("Silindi");
        }

        [HttpPost("addCar/{carId},{carModelId}")]
        [Authorize]
        public async Task<IActionResult> AddCarToModel(int carId, int carModelId)
        {
            if (!IsAdmin()) return Invalid("Yetkisiz Erişim");
            var car = await unitOfWork.Cars.GetByIdIncludedAsync(carId, u => u.CarModel);
            if (car is null) return Invalid("Araba Yok");
            var model = await unitOfWork.CarModels.GetByIdIncludedAsync(carModelId, u => u.Cars);
            if (model is null) return Invalid("Model Yok");
            
            if (model.Cars is null) 
            {
                model.Cars = new List<Car>();
            }
            if (!model.Cars.Contains(car)) 
            {
                model.Cars.Add(car);

            }

            unitOfWork.CarModels.Update(model);
            await unitOfWork.SaveChangesAsync();

            return Ok("Oluşturuldu");
        }

        [HttpDelete("deleteCar/{carId},{carModelId}")]
        [Authorize]
        public async Task<IActionResult> DeleteCarFromModel(int carId, int carModelId)
        {
            if (!IsAdmin()) return Invalid("Yetkisiz Erişim");
            var car = await unitOfWork.Cars.GetByIdIncludedAsync(carId, u => u.CarModel);
            if (car is null) return Invalid("Araba Yok");
            var model = await unitOfWork.CarModels.GetByIdIncludedAsync(carModelId, u => u.Cars);
            if (model is null) return Invalid("Model Yok");

            if (model.Cars is null)
            {
                model.Cars = new List<Car>();
            }
            if (model.Cars.Contains(car))
            {
                model.Cars.Remove(car);
            }

            unitOfWork.CarModels.Update(model);
            await unitOfWork.SaveChangesAsync();

            return Ok("Oluşturuldu");
        }

        [HttpPost("update/{carModelId}")]
        [Authorize]
        public async Task<IActionResult> UpdateCarModel([FromBody] CarModelDto carModelDto, int carModelId)
        {
            if (!IsAdmin()) return Invalid("Yetkisiz Erişim");
            if (carModelDto is null || carModelDto.Series is null || carModelDto.Model is null) return Invalid("Parametreler Eksik");
            var doesBrandExist = await unitOfWork.Brands.GetByIdIncludedAsync(carModelDto.BrandId, u => u.CarModels);
            if (doesBrandExist is null) return Invalid("Brand Yok");
            var model = await unitOfWork.CarModels.GetByIdAsync(carModelId);
            if (model is null) return Invalid("Model yok");

            model.BrandId = carModelDto.BrandId;
            model.Series = carModelDto.Series;
            model.Model = carModelDto.Model;
            model.ReleaseDate = carModelDto.ReleaseDate;
            
            unitOfWork.CarModels.Update(model);
            await unitOfWork.SaveChangesAsync();

            return Ok("Güncellendi");
        }
    }
}
