using CarGalleryHub.Application.Common.BaseDTOs;

namespace CarGalleryHub.Application.DTOs.User
{
    public class UpdateUserSecurityDto : BaseDateEntityDto 
    {
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
    }
}
