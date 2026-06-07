using CarGalleryHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
        DateTime GetExpiry(DateTime CreatedAt);
        DateTime CreateExpiry();
    }
}
