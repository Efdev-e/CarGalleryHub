using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Domain.Entities;

namespace CarGalleryHub.Application.DTOs.User.Security
{
    public class UserSecurityUpdateRequest : BaseDateEntityDto 
    {
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
    }
}
