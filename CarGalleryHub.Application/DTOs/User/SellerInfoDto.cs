using CarGalleryHub.Application.Common.BaseDTOs;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;

namespace CarGalleryHub.Application.DTOs.User
{
    public class SellerInfoDto : BaseDateEntityDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? UserId { get; set; }
    }
}
