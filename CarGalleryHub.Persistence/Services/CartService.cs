using CarGalleryHub.Application.DTOs.Cart;
using CarGalleryHub.Application.DTOs.CartItem;
using CarGalleryHub.Application.DTOs.Image;
using CarGalleryHub.Application.Exceptions;
using CarGalleryHub.Application.Interfaces;
using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
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
           
            var cart = await unitOfWork.Carts.Query()
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Thumbnail)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Advert)
                .FirstOrDefaultAsync(x => x.UserId == userId);

    
            if (cart is null)
            {
                cart = await createCartOnNull(userId, true);
            }

            var cartDto = new CartDto()
            {
                UserId = userId,
                TotalAmount = cart!.CartItems.Sum(x => x.Quantity),
                TotalPrice = cart.CartItems.Sum(x => x.Quantity * x.UnitPrice),
                CartItems = cart.CartItems.Select(x => new CartItemDto()
                {
                    Id = x.Id,
                    CartId = x.CartId,
                    AdvertId = x.AdvertId,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    ItemName = x.ItemName,
                    Thumbnail = x.Thumbnail == null ? null : new ImageDto()
                    {
                        ImageUrl = x.Thumbnail.ImageUrl,
                        ImageType = x.Thumbnail.ImageType,
                        ImageData = x.Thumbnail.ImageData,
                    }
                }).ToList()
            };

            return cartDto;
        }

        public async Task<bool> AddItemToCart(int userId,CreateCartItemDto cartItemDto)
        {
            var cart = await unitOfWork.Carts.FirstOrDefaultAsync(x => x.UserId == userId, u => u.CartItems);

            if (cart is null)
            {
                cart = await createCartOnNull(userId, true);
            }


            var advert = await unitOfWork.Adverts.GetByIdAsync(cartItemDto.AdvertId);
            if (advert is null) throw new NotFound("Advert");

            var cartItem = new CartItem()
            {
                AdvertId = cartItemDto.AdvertId,
                CartId = cart!.Id,
                Quantity = cartItemDto.Quantity,
                UnitPrice = advert.UnitPrice
            };
            var cartItems = cart.CartItems.FirstOrDefault(x => x.AdvertId == cartItem.AdvertId);
            if (cartItems is null) { cart.CartItems.Add(cartItem); }
            else { cartItems.Quantity += cartItemDto.Quantity; }
            cart.setTotalPrice();
            cart.setTotalAmount();
            unitOfWork.Carts.Update(cart);
            await unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveItemFromCart(int userId,int cartItemId)
        {
            var cart = await unitOfWork.Carts.FirstOrDefaultAsync(x => x.UserId == userId, u => u.CartItems);

            if (cart is null)
            {
                await createCartOnNull(userId, true);
                return true;
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
