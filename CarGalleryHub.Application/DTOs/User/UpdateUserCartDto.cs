using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Cart;

namespace CarGalleryHub.Application.DTOs.User
{
    public class UpdateUserCartDto : BaseDateEntityDto 
    {
        // Diğer
        public ICollection<CartDto>? Carts { get; set; }
        
    }
}
