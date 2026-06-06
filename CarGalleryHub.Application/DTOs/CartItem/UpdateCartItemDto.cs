using CarGalleryHub.Application.Common.BaseDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.CartItem
{
    public class UpdateCartItemDto : BaseDateEntityDto
    {
        public int Quantity { get; set; }
    }
}
