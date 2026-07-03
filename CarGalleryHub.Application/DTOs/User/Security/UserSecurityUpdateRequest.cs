using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Domain.Entities;

namespace CarGalleryHub.Application.DTOs.User.Security
{
    public class UserSecurityUpdateRequest 
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
