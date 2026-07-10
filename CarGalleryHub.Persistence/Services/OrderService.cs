using CarGalleryHub.Application.DTOs.Image;
using CarGalleryHub.Application.DTOs.Order;
using CarGalleryHub.Application.DTOs.OrderItem;
using CarGalleryHub.Application.Exceptions;
using CarGalleryHub.Application.Interfaces;
using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace CarGalleryHub.Persistence.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderService(IUnitOfWork work)
        {
            unitOfWork = work;
        }
        public async Task<int> CreateOrder(int userId ,CreateOrderDto orderDto)
        {
            var cart = await unitOfWork.Carts.Query().Include(x => x.CartItems).ThenInclude(x => x.Advert)
                .ThenInclude(x => x.Car)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
                    throw new AppException("No items in the bag", 404);

            
            if (orderDto is null)
                throw new MissingCredentials("Order is null");

            var user = await unitOfWork.Users.GetByIdAsync(userId);
            if (user is null) throw new NotFound("User not found");

            var aorder = new Order()
            {
                AddressCity = orderDto.AddressCity,
                AddressDistrict = orderDto.AddressDistrict,
                AddressFullName = orderDto.AddressFullName,
                AddressPostalCode = orderDto.AddressPostalCode,
                FullAddress = orderDto.FullAddress,
                UserId = user.Id,
                UserEmail = user.Email,
                UserFullName = $"{user.FirstName} {user.LastName}",
                UserPhone = user.PhoneNumber,
                OrderStatus = Domain.Enum.OrderStatus.WaitingPayment,
                OrderNumber = GenerateOrderNumber(),
                OrderItems = cart.CartItems.Select(x => new OrderItem()
                {
                    AdvertId = x.AdvertId,
                    ItemName = x.ItemName,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    Thumbnail = x.Thumbnail,
                    ImageId = x.ImageId,
                    CarColor = x.Advert.Car.Color,
                    CarKM = x.Advert.Car.KM,
                    CarYear = x.Advert.Car.Year,
                    BrandName = x.Advert.Car.BrandName,
                    ModelName = x.Advert.Car.ModelName
                }).ToList<OrderItem>(),
                
            };
            Console.WriteLine(cart.CartItems.Count);
            Console.WriteLine(cart);
            aorder.EnsureTotalCost();
            await unitOfWork.CartItems.Query().Where(x => x.CartId == cart.Id).ExecuteDeleteAsync();
            await unitOfWork.Orders.AddAsync(aorder);
            await unitOfWork.SaveChangesAsync();
            return aorder.Id;
        }

        private string GenerateOrderNumber() 
        {
            var Date = DateTime.UtcNow.ToString("yyyyMMdd");
            var rand = Random.Shared.Next(10000, 99999);
            return $"ORDER-{Date}-{rand}";
        }

        public async Task<bool> DeleteOrder(int userId, int orderId)
        {
            var order = await unitOfWork.Orders.GetByIdAsync(orderId);
            if (order is null) throw new NotFound("Not Found Order");
            if (order.UserId != userId) throw new MissingCredentials("");
            unitOfWork.Orders.Remove(order);
            await unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<List<OrderSimpleInfoDto>> GetAllOrders()
        {
            var orders = await unitOfWork.Orders.GetAllAsync();
            if (orders is null) throw new NotFound("Orders");

            var orderDto = orders.Select(x => new OrderSimpleInfoDto()
            {
                FullAddress = x.FullAddress,
                Id = x.Id,
                OrderNumber = x.OrderNumber,
                OrderStatus = x.OrderStatus,
                UserId = x.UserId
            });

            return orderDto.ToList();
            
        }

        public async Task<OrderInfoDto> GetOrderById(int userId, int orderId)
        {
            var order = await unitOfWork.Orders.Query().Include(x => x.OrderItems).ThenInclude(x => x.Thumbnail).FirstOrDefaultAsync(x => x.UserId == userId && x.Id == orderId);
            if (order is null) throw new NotFound("Order");
            if (order.UserId != userId) throw new MissingCredentials("");
            order.EnsureTotalCost();
            var orderDto = new OrderInfoDto() 
            {
                AddressCity = order.AddressCity,
                AddressDistrict = order.AddressDistrict,
                AddressFullName = order.AddressFullName,
                AddressPostalCode = order.AddressPostalCode,
                FullAddress = order.FullAddress,
                UserEmail = order.UserEmail,
                UserFullName = order.UserFullName,
                UserPhone = order.UserPhone,
                OrderStatus = order.OrderStatus,
                OrderNumber = order.OrderNumber,
                UserId = order.UserId,
                Id = order.Id,
                OrderItems = order.OrderItems.Select(x => new OrderItemDto()     
                {
                    AdvertId = x.AdvertId,
                    Quantity = x.Quantity,
                    OrderId = x.OrderId,
                    BrandName = x.BrandName,
                    CarColor = x.CarColor,
                    CarKM = x.CarKM,
                    CarYear = x.CarYear,
                    Id = x.Id,
                    ItemName = x.ItemName,
                    UnitPrice = x.UnitPrice,
                    ImageId = x.ImageId,
                    ModelName = x.ModelName,
                    Thumbnail = x.Thumbnail == null ? null : new ImageDto()
                    {
                        ImageUrl = x.Thumbnail.ImageUrl,
                        ImageType = x.Thumbnail.ImageType,
                        ImageData = x.Thumbnail.ImageData
                    },

                }).ToList()
            };
            return orderDto;
        }

        public async Task<List<OrderInfoDto>> GetUserOrders(int UserId)
        {
            return await unitOfWork.Orders.Query()
                .Include(x => x.User)
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.Thumbnail)
                .Where(x => x.UserId == UserId)
                .Select(y => new OrderInfoDto()
                {
                    Id = y.Id,
                    OrderNumber = y.OrderNumber,
                    OrderStatus = y.OrderStatus,
                    UserFullName = y.UserFullName,
                    UserPhone = y.UserPhone,
                    UserEmail = y.UserEmail,
                    AddressFullName = y.AddressFullName,
                    AddressCity = y.AddressCity,
                    AddressDistrict = y.AddressDistrict,
                    AddressPostalCode = y.AddressPostalCode,
                    FullAddress = y.FullAddress,
                    PaymentId = y.Payment != null ? y.Payment.Id : 0,
                    UserId = y.UserId,
                    OrderItems = y.OrderItems.Select(x => new OrderItemDto()
                    {
                        AdvertId = x.AdvertId,
                        Quantity = x.Quantity,
                        OrderId = x.OrderId,
                        BrandName = x.BrandName,
                        CarColor = x.CarColor,
                        CarKM = x.CarKM,
                        CarYear = x.CarYear,
                        Id = x.Id,
                        ItemName = x.ItemName,
                        UnitPrice = x.UnitPrice,
                        ImageId = x.ImageId,
                        ModelName = x.ModelName,
                        Thumbnail = x.Thumbnail == null ? null : new ImageDto()
                        {
                            ImageUrl = x.Thumbnail.ImageUrl,
                            ImageType = x.Thumbnail.ImageType,
                            ImageData = x.Thumbnail.ImageData
                        }
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<bool> UpdateOrder(int orderId,int userId, UpdateOrderDto orderStatusDto)
        {
            var order = await unitOfWork.Orders.GetByIdAsync(orderId);
            if (order is null) throw new NotFound("Order");
            if (order.UserId != userId) throw new MissingCredentials("");
            if (orderStatusDto is null) throw new MissingCredentials("Missing Credentials");
            if (orderStatusDto?.OrderStatus == null) throw new MissingCredentials("Missing Credentials");
            order.OrderStatus = orderStatusDto.OrderStatus;
            order.EnsureTotalCost();

            unitOfWork.Orders.Update(order);
            await unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
