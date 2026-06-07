using CarGalleryHub.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
        public bool VerifyPassword(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
