using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Order;

namespace CarGalleryHub.Application.DTOs.User.Orders
{
    public class UserOrderUpdateRequest : BaseDateEntityDto 
    {
        public ICollection<OrderDto>? Orders { get; set; }
    }
}
