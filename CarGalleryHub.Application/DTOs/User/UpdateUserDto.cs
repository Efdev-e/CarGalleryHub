using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Address;
using CarGalleryHub.Application.DTOs.Cart;
using CarGalleryHub.Application.DTOs.Image;
using CarGalleryHub.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.User
{
    public class UpdateUserDto : BaseDateEntityDto
    {
        // Profile
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? ImageId { get; set; }
        public ImageDto? ProfilePicture { get; set; }

        // Güvenlik

        public string? Email { get; set; }  
        public string? PhoneNumber { get; set; }  
        public string? PasswordHash { get; set; } 

        // Diğer

        // Todo: OrderDto Implement
        public ICollection<AddressDto>? Addresses { get; set; }
        public ICollection<CartDto>? Carts { get; set; } 
        public ICollection<OrderDto>? Orders { get; set; }  
    }
}
