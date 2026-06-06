using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.OrderItem;
using CarGalleryHub.Application.DTOs.Payment;
using CarGalleryHub.Application.DTOs.User;
using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.Order
{
    public class OrderDto : BaseDateEntityDto
    {
        // ----- //
        public string OrderNumber { get; set; } = string.Empty;
        public OrderStatus OrderStatus { get; set; }
        // ----- //
        public required string UserFullName { get; set; }
        public required string UserPhone { get; set; }
        public required string UserEmail { get; set; }
        public decimal TotalCost => OrderItems.Sum(x => x.UnitPrice * x.Quantity);


        // ----- //

        public required string AddressFullName { get; set; }
        public required string AddressCity { get; set; }
        public required string AddressDistrict { get; set; }
        public required string AddressPostalCode { get; set; }
        public required string FullAddress { get; set; }

        // ----- //
        public required int PaymentId { get; init; }
        public required int UserId { get; init; }
        // ----- //
        public ICollection<OrderItemDto> OrderItems { get; init; } = new List<OrderItemDto>();
        public PaymentDto Payment { get; set; } = null!;
        public UserDetailDto User { get; set; } = null!;
    }
}
