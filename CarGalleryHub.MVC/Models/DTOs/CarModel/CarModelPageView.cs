using CarGalleryHub.Application.DTOs.CarModel;

namespace CarGalleryHub.MVC.Models.DTOs.CarModel
{
    public class CarModelPageView
    {
        public int page { get; set; }
        public List<CarModelPageData> Dtos { get; set; } = new List<CarModelPageData>();
        public int totalCount { get; set; }
        public int pageSize { get; set; } = 9;
        public int TotalPages => (int)Math.Ceiling((double)totalCount / (pageSize > 0 ? pageSize : 9));
    }
}
