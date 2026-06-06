using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Car;
using CarGalleryHub.Application.DTOs.Image;
using CarGalleryHub.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.Advert
{
    public class UpdateAdvertDto : BaseDateEntityDto
    {
        public string? AdvertTitle { get; set; }
        public List<ImageDto>? Thumbnails { get; set; }
        public string? Description { get; set; }
        // ----- //
        public int? CarId { get; set; }

        // ----- //
        public CarDto Car { get; set; } = null!;
    }
}
