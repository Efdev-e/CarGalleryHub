using CarGalleryHub.Application.DTOs.Advert;
using CarGalleryHub.Application.DTOs.Car;

namespace CarGalleryHub.MVC.Models.DTOs
{
    public class AdvertViewDto
    {
        public int Id { get; set; }
        public AdvertUpdateModel updateAdvert { get; set; } = null!;
        public List<CarInfoDto>? carModels { get; set; } = null;
    }
}
