using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Brand;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.CarModel
{
    public class CreateCarModelDto
    {
        public required string Model { get; set; }
        public required string Series { get; set; }

        public required DateTime ReleaseDate { get; set; }

        // FK
        public required int BrandId { get; set; }
    }
}
