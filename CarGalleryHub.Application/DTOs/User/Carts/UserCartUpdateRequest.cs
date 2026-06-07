using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Cart;

namespace CarGalleryHub.Application.DTOs.User.Carts
{
    public class UserCartUpdateRequest : BaseDateEntityDto 
    {
        // Diğer
        public ICollection<CartDto>? Carts { get; set; }
        
    }
}
