using CarGalleryHub.Application.DTOs.Image;
using CarGalleryHub.Application.DTOs.User.Other;
using CarGalleryHub.Application.DTOs.User.Profile;
using CarGalleryHub.Application.DTOs.User.Security;
using CarGalleryHub.Application.Interfaces;
using CarGalleryHub.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarGalleryHub.Domain.Enum;
using CarGalleryHub.Application.DTOs.Dashboard;

namespace CarGalleryHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private readonly IUserService userService;
        private readonly IUnitOfWork unitOfWork;

        public UserController(IUserService s, IUnitOfWork work)
        {
            userService = s;
            unitOfWork = work;
        }

        [HttpGet("ViewProfile")]
        [Authorize]
        public async Task<IActionResult> ViewProfiles()
        {
            var profile = await userService.ViewProfile(GetUserId());
            if (profile is null) return Invalid("Profile Bulunamadı");

            var ViewDto = new ProfileViewData()
            {
                ImageUrl = profile.ProfilePicture?.ImageUrl ?? string.Empty,
                FirstName = profile.FirstName,
                LastName = profile.LastName
            };

            return Ok(ViewDto);
        }

        [HttpGet("ViewSecurity")]
        [Authorize]
        public async Task<IActionResult> ViewSecurity()
        {
            var profile = await userService.ViewSecurity(GetUserId());
            if (profile is null) return Invalid("Güvenlik Ayarları Bulunamadı");

            return Ok(profile);
        }

        [HttpPost("UpdateSecurity")]
        [Authorize]
        public async Task<IActionResult> ViewSecurity(UserSecurityPageView userSecurityPageView)
        {
            bool[] success = new bool[2];
            if (!string.IsNullOrWhiteSpace(userSecurityPageView.Email)) 
            {
                var email = await userService.ChangeEmail(GetUserId(), new UserSecurityUpdateRequest() 
                {
                    CurrentPassword = userSecurityPageView.CurrentPassword,
                    Email = userSecurityPageView.Email
                });
                if (email is false)
                    success[0] = false;
                else
                    success[0] = true;
            }
            if (!string.IsNullOrWhiteSpace(userSecurityPageView.NewPassword))
            {
                var pass = await userService.ChangePassword(GetUserId(), new UserSecurityUpdateRequest()
                {
                    CurrentPassword = userSecurityPageView.CurrentPassword,
                    NewPassword = userSecurityPageView.NewPassword
                });
                if (pass is false)
                    success[1] = false;
                else
                    success[1] = true;
            }


            return Ok(success);
        }


        [HttpGet("View")]
        [Authorize]
        public async Task<IActionResult> View()
        {
            var profile = await userService.ViewProfile(GetUserId());
            if (profile is null) return Invalid("Profile Bulunamadı");

            return Ok(profile);
        }

        [HttpPost("changeProfile")]
        [Authorize]
        public async Task<IActionResult> ChangeProfile([FromBody] UserProfileUpdateRequest userProfileUpdateRequest)
        {
            var prof = await userService.ChangeProfile(GetUserId(), userProfileUpdateRequest);
            if (!prof) return Invalid("Hata Oluştu");
            return Ok(true,"Profil Güncellendi");
        }

        [HttpPost("changePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] UserSecurityUpdateRequest updateRequest)
        {
            var prof = await userService.ChangePassword(GetUserId(), updateRequest);
            if (!prof) return Invalid("Hata Oluştu");
            return Ok(true,"Şifre değiştirildi");
        }

        [HttpPost("changeEmail")]
        [Authorize]
        public async Task<IActionResult> ChangeEmail([FromBody] UserSecurityUpdateRequest updateRequest)
        {
            var prof = await userService.ChangeEmail(GetUserId(), updateRequest);
            if (!prof) return Invalid("Hata Oluştu");
            return Ok(true,"Email değiştirildi");
        }

        [HttpPost("changePhone")]
        [Authorize]
        public async Task<IActionResult> ChangePhone([FromBody] UserSecurityUpdateRequest updateRequest)
        {
            var prof = await userService.ChangePhone(GetUserId(), updateRequest);
            if (!prof) return Invalid("Hata Oluştu");
            return Ok("Numara değiştirildi.");
        }

        [HttpGet("admin/list")]
        [Authorize]
        public async Task<IActionResult> GetUsersForAdmin()
        {
            if (!IsAdmin()) return Invalid("Yetkisiz Erişim");

            var users = await unitOfWork.Users.GetAllAsync(u => u.ProfilePicture!);
            var response = users
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Id)
                .Select(x => new UserInfoDto()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Role = x.Role,
                    ImageId = x.ImageId,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    ProfilePicture = x.ProfilePicture is null
                        ? null
                        : new ImageDto()
                        {
                            ImageType = x.ProfilePicture.ImageType,
                            ImageUrl = x.ProfilePicture.ImageUrl,
                            ImageData = x.ProfilePicture.ImageData
                        }
                })
                .ToList();

            return Ok(response);
        }

        [HttpGet("admin/stats")]
        [Authorize]
        public async Task<IActionResult> GetDashboardStats()
        {
            if (!IsAdmin()) return Invalid("Yetkisiz Erişim");

            var totalAdverts = await unitOfWork.Adverts.Query().CountAsync(x => !x.IsDeleted);
            var totalBrands = await unitOfWork.Brands.Query().CountAsync(x => !x.IsDeleted);
            var totalCars = await unitOfWork.Cars.Query().CountAsync(x => !x.IsDeleted);
            var totalUsers = await unitOfWork.Users.Query().CountAsync(x => !x.IsDeleted);
            var totalSales = await unitOfWork.Payments.Query().CountAsync(x => x.Status == PaymentStatus.Success);
            var totalEarnings = await unitOfWork.Payments.Query()
                .Where(x => x.Status == PaymentStatus.Success)
                .SumAsync(x => x.Amount);

            var stats = new AdminDashboardStatsDto
            {
                TotalAdverts = totalAdverts,
                TotalBrands = totalBrands,
                TotalCars = totalCars,
                TotalUsers = totalUsers,
                TotalSales = totalSales,
                TotalEarnings = totalEarnings
            };

            return Ok(stats);
        }

        [HttpPost("admin/update-user")]
        [Authorize]
        public async Task<IActionResult> UpdateUserForAdmin([FromBody] AdminUserUpdateDto dto)
        {
            if (!IsAdmin()) return Invalid("Yetkisiz Erişim");
            if (dto is null) return Invalid("Geçersiz veri");

            var user = await unitOfWork.Users.GetByIdAsync(dto.Id);
            if (user is null || user.IsDeleted) return Invalid("Kullanıcı bulunamadı");

            if (dto.Id == GetUserId() && dto.Role != user.Role)
            {
                return Invalid("Kendi rütbeni düşüremezsin.");
            }

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;
            user.Role = dto.Role;
            user.Updated();

            unitOfWork.Users.Update(user);
            await unitOfWork.SaveChangesAsync();

            return Ok(true, "Kullanıcı güncellendi");
        }
    }
}
