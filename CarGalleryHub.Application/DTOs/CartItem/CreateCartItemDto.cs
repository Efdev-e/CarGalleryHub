using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Advert;
using CarGalleryHub.Application.DTOs.Cart;
using CarGalleryHub.Application.DTOs.Image;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.CartItem
{
    public class CreateCartItemDto : BaseDateEntityDto
    {
        public string ItemName => Advert is null ? string.Empty : Advert.AdvertTitle;
        public ImageDto? Thumbnail { get; set; } = null;
        public decimal UnitPrice => Advert is null ? 0 : Advert.UnitPrice;
        public required int Quantity { get; set; }

        // ----- //
        public int? ImageId { get; set; }
        public required int AdvertId { get; set; }

        // ----- //
        public AdvertDto Advert { get; set; } = null!;
    }
}
