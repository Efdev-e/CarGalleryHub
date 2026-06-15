using CarGalleryHub.Application.DTOs.Image;
using CarGalleryHub.Application.DTOs.User.Profile;
using CarGalleryHub.Application.DTOs.User.Security;
using CarGalleryHub.Application.Exceptions;
using CarGalleryHub.Application.Interfaces;
using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Domain.Enum;
using CarGalleryHub.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUnitOfWork work, IPasswordHasher hasher)
        {
            _unitOfWork = work;
            _passwordHasher = hasher;
        }
        public async Task<UserProfileDto> ViewProfile(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdIncludedAsync(userId, u => u.ProfilePicture);
            if (user is null) throw new NotFound("User is null");

            var response = new UserProfileDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImageId = user.ImageId,
                ProfilePicture = new ImageDto() { ImageType = user.ProfilePicture?.ImageType ?? ImageType.Unknown, ImageUrl = user.ProfilePicture?.ImageUrl ?? "", ImageData = user.ProfilePicture?.ImageData ?? new byte[8] }
            };
            return response;
        }

        public async Task<bool> ChangeProfile(int userId ,UserProfileUpdateRequest userProfileUpdateRequest)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user is null) throw new NotFound("user");
            if (userProfileUpdateRequest is null) throw new Unsuccessful("Başarısız");


            if (!string.IsNullOrEmpty(userProfileUpdateRequest.FirstName))
                user.FirstName = userProfileUpdateRequest.FirstName;
            if (!string.IsNullOrEmpty(userProfileUpdateRequest.LastName))
                user.LastName = userProfileUpdateRequest.LastName;

            var usrPic = userProfileUpdateRequest.ProfilePicture?.ImageUrl;
            if (!string.IsNullOrEmpty(usrPic))
            {
                user.ProfilePicture ??= new Image();
                user.ProfilePicture.ImageUrl = usrPic;
                user.ProfilePicture.ImageType = ImageType.ProfilePicture;
            }
            user.Updated();
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePassword(int userId,UserSecurityUpdateRequest updateRequest)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user is null) throw new NotFound("user");
            if (updateRequest is null) throw new NotFound("Request Null");
            if (string.IsNullOrEmpty(updateRequest.CurrentPassword)) throw new NotFound("Eksik Var");
            if (string.IsNullOrEmpty(updateRequest.NewPassword)) throw new NotFound("Eksik Var");
            if (_passwordHasher.VerifyPassword(updateRequest.CurrentPassword, user.PasswordHash) == false)
                 throw new InvalidCredentials("Şifre yanlış");

            var newPass = _passwordHasher.HashPassword(updateRequest.NewPassword);
            user.PasswordHash = newPass;
            user.Updated();
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
        public async Task<bool> ChangeEmail(int userId,UserSecurityUpdateRequest updateRequest)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user is null) throw new NotFound("user");
            if (updateRequest is null) throw new NotFound("request");
            if (string.IsNullOrEmpty(updateRequest.CurrentPassword)) throw new NotFound("Eksik");
            if (string.IsNullOrEmpty(updateRequest.Email)) throw new NotFound("Eksik");
            if (_passwordHasher.VerifyPassword(updateRequest.CurrentPassword, user.PasswordHash) == false)
                throw new InvalidCredentials("Şifre yanlış");
            user.Updated();
            user.Email = updateRequest.Email;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangePhone(int userId,UserSecurityUpdateRequest updateRequest)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user is null) throw new NotFound("user");
            if (updateRequest is null) throw new NotFound("request");
            if (string.IsNullOrEmpty(updateRequest.CurrentPassword)) throw new NotFound("eksik");
            if (string.IsNullOrEmpty(updateRequest.PhoneNumber)) throw new NotFound("eksik");
            if (_passwordHasher.VerifyPassword(updateRequest.CurrentPassword, user.PasswordHash) == false)
                throw new InvalidCredentials("Şifre yanlış");

            user.PhoneNumber = updateRequest.PhoneNumber;
            user.Updated();
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
