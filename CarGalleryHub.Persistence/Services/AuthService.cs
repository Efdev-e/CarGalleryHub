using CarGalleryHub.Application.DTOs.Auth;
using CarGalleryHub.Application.Interfaces;
using CarGalleryHub.Domain.Entities;
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
        public Task<AuthResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            throw new NotImplementedException();
        }

        public Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            throw new NotImplementedException();
        }

        public AuthResponseDto ResponseBuilder(User user) => new()
        {
            
        };
    }
}
