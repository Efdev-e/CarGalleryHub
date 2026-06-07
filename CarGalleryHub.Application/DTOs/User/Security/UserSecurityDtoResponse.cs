using CarGalleryHub.Application.Common.BaseDTOs;

namespace CarGalleryHub.Application.DTOs.User.Security
{
    public class UserSecurityDtoResponse : BaseDateEntityDto
    {
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Password { get; set; }
    }
}
