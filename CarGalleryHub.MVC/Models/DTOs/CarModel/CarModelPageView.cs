using CarGalleryHub.Application.DTOs.CarModel;

namespace CarGalleryHub.MVC.Models.DTOs.CarModel
{
    public class CarModelPageView
    {
        public int page { get; set; }
        public List<CarModelPageData> Dtos { get; set; } = new List<CarModelPageData>();
    }
}
