using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Address;

namespace CarGalleryHub.Application.DTOs.User
{
    public class UpdateUserAddressDto : BaseDateEntityDto 
    {
        public ICollection<AddressDto>? Addresses { get; set; }
    }
}
