using CarGalleryHub.Application.DTOs.Cart;
using CarGalleryHub.Application.DTOs.CartItem;
using CarGalleryHub.Application.DTOs.Image;
using CarGalleryHub.Application.Interfaces;
using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Persistence.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork unitOfWork;

        public CartService(IUnitOfWork work)
        {
            unitOfWork = work;
        }

        public async Task<CartDto> GetCart(int userId)
        {
            var cart = await unitOfWork.Carts.FirstOrDefaultAsync(x => x.UserId == userId, u => u.CartItems);

            if (cart is null)
            {
                cart = await createCartOnNull(userId, true);
            }

            var cartDto = new CartDto()
            {
                CartItems = cart?.CartItems?.Select(x => new CartItemDto()
                {
                    AdvertId = x.AdvertId,
                    CartId = x.CartId,
                    Quantity = x.Quantity,
                    Id = x.Id,
                    Thumbnail = new ImageDto() { ImageUrl = x?.Thumbnail?.ImageUrl ?? "", ImageType = x?.Thumbnail?.ImageType ?? Domain.Enum.ImageType.Unknown }
                }).ToList() ?? new List<CartItemDto>(),
                UserId = userId
            };

            return cartDto;


        }

        public async Task<bool> AddItemToCart(int userId,CreateCartItemDto cartItemDto)
        {
            var cart = await unitOfWork.Carts.FirstOrDefaultAsync(x => x.UserId == userId);

            if (cart is null)
            {
                cart = await createCartOnNull(userId, false);
            }
            

            var cartItem = new CartItem()
            {
                AdvertId = cartItemDto.AdvertId,
                Cart = cart,
                Quantity = cartItemDto.Quantity
            };

            var cartItems = cart.CartItems.FirstOrDefault(x => x.AdvertId == cartItem.AdvertId);
            if (cartItems is null) { cart.CartItems.Add(cartItem); }
            else { cartItems.Quantity += 1; }

            unitOfWork.Carts.Update(cart);
            await unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveItemFromCart(int userId,int cartItemId)
        {
            var cart = await unitOfWork.Carts.FirstOrDefaultAsync(x => x.UserId == userId);

            if (cart is null)
            {
                cart = await createCartOnNull(userId, true);
            }


            var cartItem = cart.CartItems.FirstOrDefault(x => x.Id == cartItemId);
            if (cartItem is null) return true;

            if (cartItem.Quantity == 1) 
            {
                cart.CartItems.Remove(cartItem);
            }
            else { cartItem.Quantity -= 1; }

            unitOfWork.Carts.Update(cart);
            await unitOfWork.SaveChangesAsync();

            return true;
        }

        private async Task<Cart> createCartOnNull(int userId, bool SaveOrNot) 
        {
            var newCart = new Cart()
            {
                UserId = userId,
                CartItems = new List<CartItem>()
            };

            if (SaveOrNot) 
            {
                await unitOfWork.Carts.AddAsync(newCart);
                await unitOfWork.SaveChangesAsync();
            }
            
            return newCart;
        }
    }
}
