using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Brand;
using CarGalleryHub.Application.DTOs.CarModel;
using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarGalleryHub.Application.DTOs.Car
{
    public class CreateCarDto : BaseDateEntityDto
    {
        public required string BrandName { get; set; } 
        public required string ModelName { get; set; }  
        public required string Series { get; set; }

        public string MotorPower { get; set; } = string.Empty;

        // --------------------------------------------
        public required int Year { get; set; }
        public required int KM { get; set; } 
        public required ColorType Color { get; set; }
        public required CarStatus Status { get; set; }
        public required CarAvailability Availability { get; set; }

        // ----- //
        public required int CarModelId { get; set; }

        // ----- //
    }
}
