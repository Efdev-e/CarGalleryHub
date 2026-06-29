using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Brand;
using CarGalleryHub.Application.DTOs.Car;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.CarModel
{
    public class CarModelInspectDto : BaseEntityDto
    {
        public CarModelDto carModel { get; set; } = new CarModelDto();
        public string BrandName { get; set; } = string.Empty;
        public List<CarInfoDto> Cars { get; set; } = new List<CarInfoDto>();

    }
}
