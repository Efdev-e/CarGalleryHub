using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Address;

namespace CarGalleryHub.Application.DTOs.User.Address
{
    public class UserAddressDtoResponse : BaseDateEntityDto
    {
        public required ICollection<AddressDto> Addresses { get; set; }
    }
}
