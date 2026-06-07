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
        // ----- //
        public required int SellerId { get; set; }
        public required int CarId { get; set; }

        // ----- //
        public SellerInfoDto Seller { get; set; } = null!;
        public CarDto Car { get; set; } = null!;

        // ----- //

        public decimal UnitPrice => Car.UnitPrice;
    }
}
