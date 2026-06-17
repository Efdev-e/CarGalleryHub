using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.OrderItem;
using CarGalleryHub.Domain.Enum;

namespace CarGalleryHub.Application.DTOs.Order
{
    public class OrderSimpleInfoDto : BaseDateEntityDto
    {
        // ----- //
        public string OrderNumber { get; set; } = string.Empty;
        public OrderStatus OrderStatus { get; set; }
        // ----- //


        // ----- //
        public required string FullAddress { get; set; }

        // ----- //
        public int UserId { get; init; }
        // ----- /
    }
}
