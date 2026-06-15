using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Advert;
using CarGalleryHub.Application.DTOs.Brand;
using CarGalleryHub.Application.DTOs.CarModel;
using CarGalleryHub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.Car
{
    public class UpdateCarDto
    {
        public string? BrandName { get; set; } = string.Empty;
        public string? ModelName { get; set; } = string.Empty;
        public string? Series { get; set; } = string.Empty;

        public string? MotorPower { get; set; } = string.Empty;

        // --------------------------------------------
        public int? Year { get; set; }
        public int? KM { get; set; }
        public ColorType? Color { get; set; }
        public CarStatus? Status { get; set; }
        public CarAvailability? Availability { get; set; }
        public bool? Warranty { get; set; }
        public decimal? UnitPrice { get; set; }

        // ----- //
        public int? CarModelId { get; set; }
        public ICollection<AdvertDto> advertDtos { get; set; } = null!;
        // ----- //
    }
}
