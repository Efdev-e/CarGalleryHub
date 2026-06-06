using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Order;

namespace CarGalleryHub.Application.DTOs.User
{
    public class UpdateUserOrderDto : BaseDateEntityDto 
    {
        public ICollection<OrderDto>? Orders { get; set; }
    }
}
