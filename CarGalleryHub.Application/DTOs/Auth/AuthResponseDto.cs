using CarGalleryHub.Domain.Enum;

namespace CarGalleryHub.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public UserRoles Role { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
