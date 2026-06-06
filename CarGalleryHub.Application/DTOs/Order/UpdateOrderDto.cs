using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.Order
{
    public class UpdateOrderDto : BaseDateEntityDto
    {
        public OrderStatus OrderStatus { get; set; }
    }
}
