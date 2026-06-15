using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Domain.Entities;

namespace CarGalleryHub.Application.DTOs.User.Security
{
    public class UserSecurityUpdateRequest 
    {
        public required string CurrentPassword { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? NewPassword { get; set; }
    }
}
