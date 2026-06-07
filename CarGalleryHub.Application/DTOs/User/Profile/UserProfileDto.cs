using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Image;

namespace CarGalleryHub.Application.DTOs.User.Profile
{
    public class UserProfileDto : BaseDateEntityDto 
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int? ImageId { get; set; }
        public ImageDto? ProfilePicture { get; set; }
    }
}
