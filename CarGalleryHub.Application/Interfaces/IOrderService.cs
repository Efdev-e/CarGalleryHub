using CarGalleryHub.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderInfoDto> GetOrderById(int userId, int orderId);
        Task<bool> CreateOrder(int userId,CreateOrderDto orderDto);
        Task<bool> DeleteOrder(int userId, int orderId);
        Task<bool> UpdateOrder(int userId,int orderId, UpdateOrderDto orderStatusDto);
        Task<List<OrderSimpleInfoDto>> GetAllOrders();
        Task<List<OrderSimpleInfoDto>> GetUserOrders(int UserId);

    }
}
