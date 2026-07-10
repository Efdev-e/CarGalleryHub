using CarGalleryHub.Domain.Enum;

namespace CarGalleryHub.Application.DTOs.User.Other
{
    public class AdminUserUpdateDto
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public UserRoles Role { get; set; }
    }
}
