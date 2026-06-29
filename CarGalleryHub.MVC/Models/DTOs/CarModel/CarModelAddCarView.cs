using CarGalleryHub.Application.DTOs.Car;

namespace CarGalleryHub.MVC.Models.DTOs.CarModel
{
    public class CarModelAddCarView
    {
        public string ModelName { get; set; } = string.Empty;
        public int CarId { get; set; }
        public List<CarInfoDto> carDataModel { get; set; } = new List<CarInfoDto>();
    }
}
