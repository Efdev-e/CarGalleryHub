using CarGalleryHub.Application.DTOs.Image;
using CarGalleryHub.Application.DTOs.User.Profile;
using CarGalleryHub.Application.DTOs.User.Security;
using CarGalleryHub.Application.Interfaces;
using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Domain.Enum;
using CarGalleryHub.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarGalleryHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private readonly IUserService userService;

        public UserController(IUserService s)
        {
            userService = s;
        }

        // ViewProfile, ChangePassword, ChangeEmail,ChangeProfile

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewProfile() 
        {
            var profile = await userService.ViewProfile(GetUserId());
            if (profile is null) return Invalid("Profile Bulunamadı");

            return Ok(profile);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ChangeProfile(UserProfileUpdateRequest userProfileUpdateRequest)
        {
            var prof = await userService.ChangeProfile(GetUserId(), userProfileUpdateRequest);
            if (!prof) return Invalid("Hata Oluştu");
            return Ok("Profil Güncellendi");
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ChangePassword(UserSecurityUpdateRequest updateRequest) 
        {
            var prof = await userService.ChangePassword(GetUserId(), updateRequest);
            if (!prof) return Invalid("Hata Oluştu");
            return Ok("Şifre değiştirildi");
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ChangeEmail(UserSecurityUpdateRequest updateRequest)
        {
            var prof = await userService.ChangeEmail(GetUserId(), updateRequest);
            if (!prof) return Invalid("Hata Oluştu");
            return Ok("Email değiştirildi");
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ChangePhone(UserSecurityUpdateRequest updateRequest)
        {
            var prof = await userService.ChangePhone(GetUserId(), updateRequest);
            if (!prof) return Invalid("Hata Oluştu");
            return Ok("Numara değiştirildi.");
        }
    }
}
