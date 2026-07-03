using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.User.Security
{
    public class UserSecurityPageView
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string? NewPassword { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
    }
}
