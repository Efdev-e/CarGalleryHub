using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Advert;
using CarGalleryHub.Application.DTOs.Car;
using CarGalleryHub.Application.DTOs.Cart;
using CarGalleryHub.Application.DTOs.CartItem;
using CarGalleryHub.Application.DTOs.OrderItem;
using CarGalleryHub.Application.DTOs.User;
using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarGalleryHub.Application.DTOs.Image
{
    public class ImageDto
    {
        public string ImageUrl { get; set; } = string.Empty;
        public ImageType ImageType { get; set; }
    }
}
