using CarGalleryHub.Application.Common.BaseDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.Brand
{
    public class BrandInfoDto : BaseEntityDto
    {
        public required string BrandName { get; set; }
    }
}
