using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Car;
using CarGalleryHub.Application.DTOs.Image;
using CarGalleryHub.Application.DTOs.User.Other;
using CarGalleryHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.Advert
{
    public class AdvertDto : BaseDateEntityDto
    {
        public required string AdvertTitle { get; set; }
        public List<ImageDto> Thumbnails { get; set; } = new List<ImageDto>();
        public required string Description { get; set; }
        public required bool Warranty { get; set; } = false;
        // ----- //
        public required int SellerId { get; set; }
        public required int CarId { get; set; }
        public string? CarName { get; set; }

        // ----- //
        public int KM { get; set; }
        public int Year { get; set; }
        public string? Color { get; set; }
        public string? Status { get; set; }
        public string? Availability { get; set; }
        public string? MotorPower { get; set; }
        public string? BrandName { get; set; }
        public string? ModelName { get; set; }
        public string? Series { get; set; }
        public string? GearType { get; set; }
        public string? FuelType { get; set; }
        public string? PaintStatus { get; set; }

        // ----- //

        public required decimal UnitPrice { get; set; }
    }
}
