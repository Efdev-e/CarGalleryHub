using CarGalleryHub.Application.DTOs.User.Profile;
using CarGalleryHub.Application.DTOs.User.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileDto> ViewProfile(int userId);
        Task<bool> ChangeProfile(int userId, UserProfileUpdateRequest updateDto);
        Task<bool> ChangePassword(int userId, UserSecurityUpdateRequest requestDto);
        Task<bool> ChangeEmail(int userId, UserSecurityUpdateRequest requestDto);
        Task<bool> ChangePhone(int userId, UserSecurityUpdateRequest requestDto);


    }
}
