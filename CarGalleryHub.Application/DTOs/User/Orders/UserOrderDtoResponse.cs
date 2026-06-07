using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Order;

namespace CarGalleryHub.Application.DTOs.User.Orders
{
    public class UserOrderDtoResponse : BaseDateEntityDto
    {
        public required ICollection<OrderDto> Orders { get; set; }
    }
}
