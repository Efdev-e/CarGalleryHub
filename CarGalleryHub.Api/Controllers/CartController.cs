using CarGalleryHub.Application.DTOs.Car;
using CarGalleryHub.Application.DTOs.Cart;
using CarGalleryHub.Application.DTOs.CartItem;
using CarGalleryHub.Application.Interfaces;
using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Persistence.Services;
using CarGalleryHub.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace CarGalleryHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : BaseApiController
    {
        private readonly ICartService cartService;

        public CartController(ICartService work)
        {
            cartService = work;
        }

        [HttpGet("GetCart")]
        [Authorize]
        public async Task<IActionResult> GetCart() 
        {
            var cartDto = await cartService.GetCart(GetUserId());
            if (cartDto is null) return Invalid("Error");
            return Ok(cartDto);
        }

        [HttpPut("addItem")]
        [Authorize]
        public async Task<IActionResult> AddItemToCart([FromBody] CreateCartItemDto cartItemDto) 
        {
            var cart = await cartService.AddItemToCart(GetUserId(), cartItemDto);
            if (!cart) return Invalid("Err");
            return Ok();
        }

        [HttpDelete("removeItem/{cartItemId}")]
        [Authorize]
        public async Task<IActionResult> RemoveItemFromCart(int cartItemId)
        {
            var cart = await cartService.RemoveItemFromCart(GetUserId(), cartItemId);
            if (!cart) return Invalid("err");
            return Ok();
        }
    }
}
