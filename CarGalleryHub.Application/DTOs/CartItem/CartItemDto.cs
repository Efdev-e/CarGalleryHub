using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Advert;
using CarGalleryHub.Application.DTOs.Cart;
using CarGalleryHub.Application.DTOs.Image;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.CartItem
{
    public class CartItemDto : BaseDateEntityDto
    {
        public string ItemName => Advert.AdvertTitle;
        public ImageDto? Thumbnail { get; set; } = null;
        public decimal UnitPrice => Advert.UnitPrice;
        public required int Quantity { get; set; }

        // ----- //
        public int? ImageId { get; set; }
        public required int AdvertId { get; set; }
        public required int CartId { get; set; }

        // ----- //
        public AdvertDto Advert { get; set; } = null!;
        public CartDto Cart { get; set; } = null!;
    }
}
