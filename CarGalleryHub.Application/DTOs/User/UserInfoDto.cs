using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Address;
using CarGalleryHub.Application.DTOs.Cart;
using CarGalleryHub.Application.DTOs.Image;
using CarGalleryHub.Application.DTOs.Order;
using CarGalleryHub.Domain.Enum;

namespace CarGalleryHub.Application.DTOs.User
{
    public class UserInfoDto : BaseDateEntityDto 
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? ImageId { get; set; }
        public ImageDto? ProfilePicture { get; set; }
        public UserRoles Role { get; set; }

        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public ICollection<AddressDto>? Addresses { get; set; }
        public ICollection<CartDto>? Carts { get; set; }
        public ICollection<OrderDto>? Orders { get; set; }
    }
}
