using CarGalleryHub.Application.DTOs.Auth;
using CarGalleryHub.Application.Exceptions;
using CarGalleryHub.Application.Interfaces;
using CarGalleryHub.Domain.Entities;
using CarGalleryHub.Domain.Enum;
using CarGalleryHub.Domain.Exceptions;
using CarGalleryHub.Persistence.Context;
using CarGalleryHub.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Persistence.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IPasswordHasher _hasher;
        private readonly IJwtTokenService _jwtTokenService;


        public AuthService(IUnitOfWork context, IPasswordHasher passwordHasher, IJwtTokenService jwtTokenService)
        {
            _unitofwork = context;
            _hasher = passwordHasher;
            _jwtTokenService = jwtTokenService;
        }
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            if (string.IsNullOrWhiteSpace(loginRequestDto.Email))
                throw new MissingCredentials("Email Credential");
            if (string.IsNullOrWhiteSpace(loginRequestDto.Password))
                throw new MissingCredentials("Password Credential");

            var user = await _unitofwork.Users.FirstOrDefaultAsync(x => x.Email == loginRequestDto.Email);
            if (user is null)
                throw new NotFound("User");


            return ResponseBuilder(user);
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            if (string.IsNullOrWhiteSpace(registerRequestDto.Email))
                throw new MissingCredentials("Email Credential");
            if (string.IsNullOrWhiteSpace(registerRequestDto.Password))
                throw new MissingCredentials("Password Credential");
            if (string.IsNullOrEmpty(registerRequestDto.FirstName) || string.IsNullOrEmpty(registerRequestDto.LastName))
                throw new InvalidCredentials("First Name Credential");


            var user = new User()
            {
                FirstName = registerRequestDto.FirstName,
                LastName = registerRequestDto.LastName,
                Email = registerRequestDto.Email,
                PasswordHash = _hasher.HashPassword(registerRequestDto.Password),
            };

            await _unitofwork.Users.AddAsync(user);
            await _unitofwork.SaveChangesAsync();

            return ResponseBuilder(user);
        }

        public AuthResponseDto ResponseBuilder(User user) => new()
        {
            Token = _jwtTokenService.GenerateToken(user),
            Email = user.Email,
            ExpiresAt = _jwtTokenService.GetExpiry(DateTime.UtcNow),
            FullName = $"{user.FirstName} {user.LastName}",
            Role = user.Role
        };
    }
}
