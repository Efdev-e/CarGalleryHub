using CarGalleryHub.Application.DTOs.Brand;
using CarGalleryHub.Application.DTOs.CarModel;

namespace CarGalleryHub.MVC.Models.DTOs.CarModel
{
    public class CarModelUpdateView
    {
        public int CarModelId { get; set; }
        public CarModelDataCreate CarModelData { get; set; } = new();

        public List<BrandListDto> Brands { get; set; } = new List<BrandListDto>();
    }
}
