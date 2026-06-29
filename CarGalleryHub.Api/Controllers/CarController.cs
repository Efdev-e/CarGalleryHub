using CarGalleryHub.Application.DTOs.Advert;
using CarGalleryHub.Application.DTOs.Car;
using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Domain.Enum;
using CarGalleryHub.Persistence.UnitOfWork;
using Iyzipay.Request.V2.Subscription;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarGalleryHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly int PageNumber = 10;

        public CarController(IUnitOfWork work)
        {
            unitOfWork = work;
        }

        [HttpGet("GetAllCarms")]
        [Authorize]
        public async Task<IActionResult> GetAllCarms()
        {
            var query = unitOfWork.Cars.Query();
            query = query.Where(x => x.IsDeleted == false);
            var dto = query.Select(x => new CarInfoDto()
            {
                FullName = $"{x.BrandName} {x.ModelName} {x.Color} {x.Year}",
                Id = x.Id
            }).ToList();

            if (dto is null) 
            {
                return NotFound(" Araba yok");
            }

            return Ok(dto);

        }

        [HttpGet("GetAllCarms/{Page:int}")]
        [Authorize]
        public async Task<IActionResult> GetAllCarms(int Page, [FromQuery] CarAvailability[]? availability, [FromQuery] ColorType[]? color, [FromQuery] CarStatus[]? status, [FromQuery] string? Name)
        {
            var query = unitOfWork.Cars.Query();
            query = query.Where(x => x.IsDeleted == false);

            if (availability is not null && availability.Length > 0)
            {
                query = query.Where(x => availability.Contains(x.Availability));
            }

            if (color is not null && color.Length > 0)
            {
                query = query.Where(x => color.Contains(x.Color));
            }

            if (status is not null && status.Length > 0)
            {
                query = query.Where(x => status.Contains(x.Status));
            }

            if (!string.IsNullOrEmpty(Name))
            {
                query = query.Where(x =>
                    EF.Functions.Like(x.BrandName, $"%{Name}%") ||
                    EF.Functions.Like(x.ModelName, $"%{Name}%") ||
                    EF.Functions.Like(x.Color.ToString(), $"%{Name}%")
                );
            }


            var cars = await query
                .OrderBy(x => x.Id)
                .Skip((Page - 1) * PageNumber)
                .Take(PageNumber)
                .ToListAsync();

            if (!cars.Any())
                return Invalid("Araba Yok");

            var dto = cars.Select(x => new CarInfoDto()
            {
                FullName = $"{x.BrandName} {x.ModelName} {x.Color} {x.Year}",
                Id = x.Id
            }).ToList();

            return Ok(dto);

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCarsCreatedByUser() 
        {
            var Cars = await unitOfWork.Cars.FindAsync(x => x.UserIds.Contains(GetUserId()), x => x.UserIds);
            if (Cars is null) 
            {
                return NotFound("Kullanıcı araba oluşturmamış");
            }

            var dto = Cars.Select(x => new CarInfoDto() 
            {
                FullName = $"{x.BrandName} {x.ModelName} {x.Color} {x.Year}",
                Id = x.Id
            }).ToList();

            return Ok(dto);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById(int id) 
        {
            var Car = await unitOfWork.Cars.GetByIdAsync(id);
            if (Car is null) return Invalid("Car Yok");
            if (Car.IsDeleted) return Invalid("Araba Silinmiş");
            var dto = new CarDto() 
            {
                Availability = Car.Availability,
                BrandName = Car.BrandName,
                CarModelId = Car.CarModelId,
                Color = Car.Color,
                KM = Car.KM,
                ModelName = Car.ModelName,
                Series = Car.Series,
                Status = Car.Status,
                Year = Car.Year,
                CreatedAt = Car.CreatedAt,
                Id = Car.Id,
                MotorPower = Car.MotorPower,
                UpdatedAt = Car.UpdatedAt
            };

            return Ok(dto);
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateCar([FromBody] CarDto carDto) 
        {
            if (carDto is null) return Invalid();
            var carExist = await unitOfWork.Cars.FirstOrDefaultAsync(x => 
                           x.CarModelId == carDto.CarModelId &&
                           x.Color == carDto.Color &&
                           x.Year == carDto.Year &&
                           x.KM == carDto.KM &&
                           x.Status == carDto.Status &&
                           x.Availability == carDto.Availability);

            if (carExist is not null) 
            {
                carExist.UserIds.Add(GetUserId());
                unitOfWork.Cars.Update(carExist);
                await unitOfWork.SaveChangesAsync();
                return Ok(true);
            }

            var car = new Car()
            {
                Availability = carDto.Availability,
                BrandName = carDto.BrandName,
                CarModelId = carDto.CarModelId,
                Color = carDto.Color,
                KM = carDto.KM,
                ModelName = carDto.ModelName,
                Series = carDto.Series,
                Status = carDto.Status,
                Year = carDto.Year,
                MotorPower = carDto.MotorPower
            };

            await unitOfWork.Cars.AddAsync(car);
            await unitOfWork.SaveChangesAsync();
            return Ok(true);
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCarById(int id) 
        {
            if (!IsAdmin()) return Invalid("Yetkisiz Erişim");
            var carexist = await unitOfWork.Cars.GetByIdAsync(id);
            if (carexist is null) return Invalid("Car yok");

            carexist.DeleteCar();
            unitOfWork.Cars.Update(carexist);
            await unitOfWork.SaveChangesAsync();

            return Ok(true);
        }

        [HttpPost("update/{carid}")]
        [Authorize]
        public async Task<IActionResult> UpdateCar([FromBody] UpdateCarDto carDto, int carid)
        {
            if (!IsAdmin()) return Invalid("Yetkisiz Erişim");

            if (carDto is null) return Invalid();
            var car = await unitOfWork.Cars.GetByIdAsync(carid);
            if (car is null) return Invalid("Araba yok");

            car.KM = carDto?.KM ?? car.KM;
            car.Color = carDto?.Color ?? car.Color  ;
            car.Status = carDto?.Status ?? car.Status;
            car.MotorPower = carDto?.MotorPower ?? car.MotorPower;
            car.Year = carDto?.Year ?? car.Year;
            car.Availability = carDto?.Availability ?? car.Availability;
            if (carDto?.CarModelId is int carModelId)
            {
                var model = await unitOfWork.CarModels.GetByIdIncludedAsync(carModelId, x => x.Brand);
                if (model is null) return Invalid("Model yok");

                car.CarModelId = model.Id;
                car.BrandName = model.Brand?.BrandName ?? car.BrandName;
                car.ModelName = model.Model;
                car.Series = model.Series;
            }
            
            unitOfWork.Cars.Update(car);
            await unitOfWork.SaveChangesAsync();

            return Ok(true);
        }
    }
}
