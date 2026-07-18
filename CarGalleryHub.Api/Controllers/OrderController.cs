using CarGalleryHub.Application.DTOs.Order;
using CarGalleryHub.Application.Interfaces;
using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalleryHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseApiController
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderS)
        {
            orderService = orderS;          
        }


        [HttpGet("[action]/{orderId}")]
        [Authorize]
        public async Task<IActionResult> GetOrderById(int orderId) 
        {
            var result = await orderService.GetOrderById(GetUserId(), orderId);
            if (result is null) return BadRequest();

            return Ok(result);
        }

        [HttpGet("GetOrderByIdForAdmin/{orderId}")]
        [Authorize]
        public async Task<IActionResult> GetOrderByIdForAdmin(int orderId)
        {
            if (!IsAdmin()) return BadRequest();
            var result = await orderService.GetOrderByIdForAdmin(orderId);
            if (result is null) return BadRequest();

            return Ok(result);
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto) 
        {
            var result = await orderService.CreateOrder(GetUserId(), createOrderDto);
            if (result <= 0) return BadRequest();

            return Ok(result);
        }

        [HttpDelete("[action]/{orderId}")]
        [Authorize]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var result = await orderService.DeleteOrder(GetUserId(), orderId);
            if (!result) return BadRequest();

            return Ok(result);
        }

        [HttpPost("[action]/{orderId}")]
        [Authorize]
        public async Task<IActionResult> UpdateOrder(int orderId,[FromBody] UpdateOrderDto updateOrderDto)
        {
            var result = await orderService.UpdateOrder(GetUserId(),orderId, updateOrderDto);
            if (!result) return BadRequest();

            return Ok(result);
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> GetAllOrders()
        {
            if (!IsAdmin()) return BadRequest();
            var result = await orderService.GetAllOrders();
            if (result is null) return BadRequest();

            return Ok(result);
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> GetUserOrders()
        {
            var result = await orderService.GetUserOrders(GetUserId());
            if (result is null) return BadRequest();

            return Ok(result);
        }
    }
}
