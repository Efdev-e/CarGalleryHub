using CarGalleryHub.Application.Interfaces;
using CarGalleryHub.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using CarGalleryHub.Infrastructure.Options;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace CarGalleryHub.Infrastructure.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtOptions _options;
        public JwtTokenService(IOptions<JwtOptions> options )
        {
            _options = options.Value;
        }
        public string GenerateToken(User user)
        {
            var claimss = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                    issuer: _options.Issuer,
                    audience: _options.Audience,
                    claims: claimss,
                    signingCredentials: creds,
                    expires: CreateExpiry()
                );
            

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public DateTime CreateExpiry() => DateTime.UtcNow.AddMinutes(_options.ExpiresInMinutes);

        public DateTime GetExpiry(DateTime CreatedAt) => CreatedAt.AddMinutes(_options.ExpiresInMinutes);

    }
}
