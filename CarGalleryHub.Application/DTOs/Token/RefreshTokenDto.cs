using CarGalleryHub.Application.DTOs.User.Other;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.Token
{
    public class RefreshTokenDto
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public DateTime CreatedAt => DateTime.UtcNow;
        public DateTime? RevokedAt { get; set; }
        public bool IsActive => !IsExpired && RevokedAt == null;

        //

        public int UserId { get; set; }
        public UserInfoDto User { get; set; } = null!;

    }
}
