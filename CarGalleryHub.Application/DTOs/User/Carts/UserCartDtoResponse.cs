using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Cart;

namespace CarGalleryHub.Application.DTOs.User.Carts
{
    public class UserCartDtoResponse : BaseDateEntityDto
    {
        public required ICollection<CartDto> Carts { get; set; }

    }
}
