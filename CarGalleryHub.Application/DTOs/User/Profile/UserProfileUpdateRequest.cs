using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Image;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.User.Profile
{
    public class UserProfileUpdateRequest : BaseDateEntityDto
    {
        // Profile
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? ImageId { get; set; }
        public ImageDto? ProfilePicture { get; set; }
    }
}
