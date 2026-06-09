using CarGalleryHub.Application.DTOs.Auth;
using CarGalleryHub.Application.Exceptions;
using CarGalleryHub.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarGalleryHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService service)
        {
            authService = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto) 
        {
            var result = await authService.RegisterAsync(registerRequestDto);
            return Created(result, "Kayıt Başarılıdır");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto) 
        {
            var result = await authService.LoginAsync(loginRequestDto);
            if (result is null) return Invalid(data: "None");
            return Ok(result);
        }
    }
}
