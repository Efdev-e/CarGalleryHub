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
    public class UserDetailDto : BaseDateEntityDto
    {
        // Profile
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int? ImageId { get; set; }
        public ImageDto? ProfilePicture { get; set; }

        // Güvenlik

        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        // Diğer

        // Todo: OrderDto Implement
        public ICollection<AddressDto> Addresses { get; set; } = new List<AddressDto>();
        public ICollection<CartDto> Carts { get; set; } = new List<CartDto>();
        public ICollection<OrderDto> Orders { get; set; } = new List<OrderDto>();
    }
}
