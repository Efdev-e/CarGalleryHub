using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Car;
using CarGalleryHub.Application.DTOs.Cart;
using CarGalleryHub.Application.DTOs.User;
using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarGalleryHub.Application.DTOs.Image
{
    public class ImageDto : BaseEntityDto
    {
        public string ImageUrl { get; set; } = string.Empty;
        public ImageType ImageType { get; set; } = ImageType.Unknown;

        // FK

        public int? UserId { get; set; }
        public UserDetailDto? User { get; set; }

        public int? CarId { get; set; }
        public CarDto? Car { get; set; }

        public int? CartId { get; set; }
        public CartDto? Cart { get; set; }
    }
}
