using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.User;

namespace CarGalleryHub.Application.DTOs.Address
{
    public class AddressDto : BaseEntityDto
    {
        public required string FullName { get; set; }  
        public required string Phone { get; set; }
        public string Email { get; set; } = string.Empty;
        public required string City { get; set; } 
        public required string District { get; set; } 
        public required string PostalCode { get; set; } 
        public required string FullAddress { get; set; } 
        public ICollection<UserDetailDto> Users { get; set; } = new List<UserDetailDto>();
    }
}
