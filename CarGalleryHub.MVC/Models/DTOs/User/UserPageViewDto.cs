using CarGalleryHub.Application.DTOs.User.Other;

namespace CarGalleryHub.MVC.Models.DTOs.User
{
    public class UserPageViewDto
    {
        public List<UserInfoDto> Users { get; set; } = new();
    }
}
