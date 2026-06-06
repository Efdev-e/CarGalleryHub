using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Brand;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.CarModel
{
    public class CarModelDto : BaseEntityDto
    {
        // Araba Mdl
        public string BrandName { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Series { get; set; } = string.Empty;

        public DateTime ReleaseDate { get; set; }

        // FK
        public required int BrandId { get; set; }
        public BrandDto Brand { get; set; } = null!;
    }
}
