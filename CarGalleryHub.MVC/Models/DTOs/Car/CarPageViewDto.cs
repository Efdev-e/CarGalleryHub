using CarGalleryHub.Application.DTOs.Car;

namespace CarGalleryHub.MVC.Models.DTOs.Car
{
    public class CarPageViewDto
    {
        public int page { get; set; }
        public List<CarInfoDto> Dtos { get; set; } = new();
    }
}
