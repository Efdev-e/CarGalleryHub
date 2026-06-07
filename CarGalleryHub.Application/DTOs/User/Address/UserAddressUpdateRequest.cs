using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Address;

namespace CarGalleryHub.Application.DTOs.User.Address
{
    public class UserAddressUpdateRequest : BaseDateEntityDto 
    {
        public ICollection<AddressDto>? Addresses { get; set; }
    }
}
