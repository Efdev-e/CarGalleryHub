using CarGalleryHub.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
    }
}
