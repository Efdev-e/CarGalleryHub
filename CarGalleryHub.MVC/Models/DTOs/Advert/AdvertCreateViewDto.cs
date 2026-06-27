using CarGalleryHub.Application.DTOs.Address;
using CarGalleryHub.Application.DTOs.Advert;
using CarGalleryHub.Application.DTOs.Car;

namespace CarGalleryHub.MVC.Models.DTOs.Advert
{
    public class AdvertCreateViewDto
    {
        public CreateAdvertDto createAdvertDto { get; set; } = null!;
        public List<CarInfoDto> Cars { get; set; } = null!;
    }
}
