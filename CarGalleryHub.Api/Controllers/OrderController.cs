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
            var result = orderService.GetOrderById(GetUserId(), orderId);
            if (result is null) return BadRequest();

            return Ok(result);
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto) 
        {
            var result = orderService.CreateOrder(GetUserId(), createOrderDto);
            if (result is null) return BadRequest();

            return Ok(result);
        }

        [HttpDelete("[action]/{orderId}")]
        [Authorize]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var result = orderService.DeleteOrder(GetUserId(), orderId);
            if (result is null) return BadRequest();

            return Ok(result);
        }

        [HttpPost("[action]/{orderId}")]
        [Authorize]
        public async Task<IActionResult> UpdateOrder(int orderId,[FromBody] UpdateOrderDto updateOrderDto)
        {
            var result = orderService.UpdateOrder(GetUserId(),orderId, updateOrderDto);
            if (result is null) return BadRequest();

            return Ok(result);
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> GetAllOrders()
        {
            if (!IsAdmin()) return BadRequest();
            var result = orderService.GetAllOrders();
            if (result is null) return BadRequest();

            return Ok(result);
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> GetUserOrders()
        {
            var result = orderService.GetUserOrders(GetUserId());
            if (result is null) return BadRequest();

            return Ok(result);
        }
    }
}
