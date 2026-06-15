using CarGalleryHub.Application.DTOs.Cart;
using CarGalleryHub.Application.DTOs.CartItem;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CarGalleryHub.Application.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCart(int userId);
        Task<bool> AddItemToCart(int userId, CreateCartItemDto cartItemDto);
        Task<bool> RemoveItemFromCart(int userId, int cartItemId);
    }
}
