using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.CarModel;
using CarGalleryHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.Brand
{
    public class BrandDto : BaseEntityDto
    {
        public required string BrandName { get; set; }
        public ICollection<CarModelDto> CarModels { get; set; } = new List<CarModelDto>();
    }
}
