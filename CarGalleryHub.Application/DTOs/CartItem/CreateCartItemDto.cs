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
        public required int Quantity { get; set; }

        // ----- //
        public int? ImageId { get; set; }
        public required int AdvertId { get; set; }

        // ----- //
    }
}
