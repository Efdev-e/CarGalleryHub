using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.Marshalling;
using System.Text;

namespace CarGalleryHub.Application.DTOs.Auth
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "E-posta Gerekli")]
        [EmailAddress(ErrorMessage = "E-posta formatı yanlış")]
        [StringLength(64,ErrorMessage = "E-postanız çok uzun")]
        public string Email { get; set; } = string.Empty;

        [StringLength(64, ErrorMessage = "Şifreniz Çok Uzun")]
        [Required(ErrorMessage = "Şifreniz Gerekli")]
        public string Password { get; set; } = string.Empty;
    }
}
