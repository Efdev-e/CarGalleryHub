using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Image;
using CarGalleryHub.Domain.Enum;
using CarGalleryHub.Application.DTOs.Token;

namespace CarGalleryHub.Application.DTOs.User.Token
{
    public class CreateUserDto : BaseDateEntityDto
    {
        // Profile
        public required string FirstName { get; set; }  
        public required string LastName { get; set; }  
        public int? ImageId { get; set; }
        public ImageDto? ProfilePicture { get; set; }
        public UserRoles Role { get; set; }

        // Güvenlik

        public required string Email { get; set; } 
        public required string PhoneNumber { get; set; } 
        public required string PasswordHash { get; set; } 

        // Diğer

    }
}
