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
    public class UserController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public UserController(IUnitOfWork work, IPasswordHasher hasher)
        {
            _unitOfWork = work;
            _passwordHasher = hasher;
        }

        // ViewProfile, ChangePassword, ChangeEmail,ChangeProfile

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewProfile() 
        {
            var user = await _unitOfWork.Users.GetByIdAsync(GetUserId());
            if (user is null) return Invalid("Kullanıcı Alınamadı");

            var response = new UserProfileDto() 
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImageId = user.ImageId,
                ProfilePicture = new ImageDto() { ImageType = user.ProfilePicture?.ImageType ?? ImageType.Unknown , ImageUrl = user.ProfilePicture?.ImageUrl ?? "" }
            };
            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangeProfile(UserProfileUpdateRequest userProfileUpdateRequest)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(GetUserId());
            if (user is null) return Invalid("Kullanıcı Alınamadı");
            if (userProfileUpdateRequest is null) return Invalid("Başarız");


            if (!string.IsNullOrEmpty(userProfileUpdateRequest.FirstName))
                user.FirstName = userProfileUpdateRequest.FirstName;
            if(!string.IsNullOrEmpty(userProfileUpdateRequest.LastName))
                user.LastName = userProfileUpdateRequest.LastName;

            var usrPic = userProfileUpdateRequest.ProfilePicture?.ImageUrl;
            if (!string.IsNullOrEmpty(usrPic))
            {
                user.ProfilePicture ??= new Image();
                user.ProfilePicture.ImageUrl = usrPic;
                user.ProfilePicture.ImageType = ImageType.ProfilePicture;
            }

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ChangePassword(UserSecurityUpdateRequest updateRequest) 
        {
            var user = await _unitOfWork.Users.GetByIdAsync(GetUserId());
            if (user is null) return Invalid("Kullanıcı Bulunamadı");
            if (updateRequest is null) return Invalid("Request null");
            if (string.IsNullOrEmpty(updateRequest.CurrentPassword)) return Invalid("Eski Şifre Girilmedi");
            if (string.IsNullOrEmpty(updateRequest.NewPassword)) return Invalid("Yeni Şifre Girilmedi");
            if (_passwordHasher.VerifyPassword(updateRequest.CurrentPassword, user.PasswordHash) == false)
                return Invalid("Şifre Yanlış");
            
            var newPass = _passwordHasher.HashPassword(updateRequest.NewPassword);
            user.PasswordHash = newPass;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return Ok("Şifre Değiştirildi.");
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ChangeEmail(UserSecurityUpdateRequest updateRequest)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(GetUserId());
            if (user is null) return Invalid("Kullanıcı Bulunamadı");
            if (updateRequest is null) return Invalid("Request null");
            if (string.IsNullOrEmpty(updateRequest.CurrentPassword)) return Invalid("Eski Şifre Girilmedi");
            if (string.IsNullOrEmpty(updateRequest.Email)) return Invalid("Yeni Email Girilmedi");
            if (_passwordHasher.VerifyPassword(updateRequest.CurrentPassword, user.PasswordHash) == false)
                return Invalid("Şifre Yanlış");

            user.Email = updateRequest.Email;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return Ok("Email Değiştirildi.");
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ChangePhone(UserSecurityUpdateRequest updateRequest)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(GetUserId());
            if (user is null) return Invalid("Kullanıcı Bulunamadı");
            if (updateRequest is null) return Invalid("Request null");
            if (string.IsNullOrEmpty(updateRequest.CurrentPassword)) return Invalid("Eski Şifre Girilmedi");
            if (string.IsNullOrEmpty(updateRequest.PhoneNumber)) return Invalid("Yeni Telefon Numarası Girilmedi");
            if (_passwordHasher.VerifyPassword(updateRequest.CurrentPassword, user.PasswordHash) == false)
                return Invalid("Şifre Yanlış");

            user.PhoneNumber = updateRequest.PhoneNumber;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return Ok("Numaranız Değiştirildi.");
        }
    }
}
