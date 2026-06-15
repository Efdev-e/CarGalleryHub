using CarGalleryHub.Application.DTOs.Car;
using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Persistence.UnitOfWork;
using Iyzipay.Request.V2.Subscription;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalleryHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : BaseApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public CarController(IUnitOfWork work)
        {
            unitOfWork = work;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById(int id) 
        {
            var Car = await unitOfWork.Cars.GetByIdAsync(id);
            if (Car is null) return Invalid("Car Yok");
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

            if (carExist is not null) { return Ok(); }

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
            return Ok("Oluşturuldu");
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCarById(int id) 
        {
            if (!IsAdmin()) return Invalid("Yetkisiz Erişim");
            var carexist = await unitOfWork.Cars.GetByIdAsync(id);
            if (carexist is null) return Invalid("Car yok");

            unitOfWork.Cars.Remove(carexist);
            await unitOfWork.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("update/{carid}")]
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
            
            unitOfWork.Cars.Update(car);
            await unitOfWork.SaveChangesAsync();

            return Ok("Güncellendi");
        }
    }
}
