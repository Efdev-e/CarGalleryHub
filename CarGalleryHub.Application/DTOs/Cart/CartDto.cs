using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.CartItem;
using CarGalleryHub.Application.DTOs.User;
using CarGalleryHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.Cart
{
    public class CartDto : BaseDateEntityDto
    {
        public ICollection<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
        public required int UserId { get; set; }

        public int TotalAmount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
