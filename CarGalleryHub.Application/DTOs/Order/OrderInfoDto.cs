using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.OrderItem;
using CarGalleryHub.Domain.Enum;

namespace CarGalleryHub.Application.DTOs.Order
{
    public class OrderInfoDto : BaseDateEntityDto
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
        public int PaymentId { get; init; }
        public int UserId { get; init; }
        // ----- //
        public ICollection<OrderItemDto> OrderItems { get; init; } = new List<OrderItemDto>();
    }
}
