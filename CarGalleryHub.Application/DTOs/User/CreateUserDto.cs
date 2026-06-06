using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Address;
using CarGalleryHub.Application.DTOs.Cart;
using CarGalleryHub.Application.DTOs.Image;
using CarGalleryHub.Application.DTOs.Order;
using CarGalleryHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.User
{
    public class CreateUserDto : BaseDateEntityDto
    {
        // Profile
        public required string FirstName { get; set; }  
        public required string LastName { get; set; }  
        public int? ImageId { get; set; }
        public ImageDto? ProfilePicture { get; set; }

        // Güvenlik

        public required string Email { get; set; } 
        public required string PhoneNumber { get; set; } 
        public required string Password { get; set; } 

        // Diğer

        // Todo: OrderDto Implement
        public ICollection<AddressDto> Addresses { get; set; } = new List<AddressDto>();
        public ICollection<CartDto> Carts { get; set; } = new List<CartDto>();
        public ICollection<OrderDto> Orders { get; set; } = new List<OrderDto>();
    }
}
