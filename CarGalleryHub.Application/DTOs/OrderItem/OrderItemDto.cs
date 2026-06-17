using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Advert;
using CarGalleryHub.Application.DTOs.Cart;
using CarGalleryHub.Application.DTOs.Image;
using CarGalleryHub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.OrderItem
{
    public class OrderItemDto : BaseDateEntityDto
    {
        public string ItemName {get; set;} = null!;
        public ImageDto? Thumbnail { get; set; } = null!;
        public decimal UnitPrice { get; set;}
        public required int Quantity { get; set; }

        public int CarYear { get; set; }
        public int CarKM { get; set; }
        public ColorType CarColor { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;

        // ----- //
        public int? ImageId { get; set; }
        public required int AdvertId { get; set; }
        public required int OrderId { get; set; }

        // ----- //
        public AdvertDto? Advert { get; set; }
    }
}
