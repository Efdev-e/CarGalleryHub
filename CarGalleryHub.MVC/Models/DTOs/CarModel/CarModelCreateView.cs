using CarGalleryHub.Application.DTOs.Brand;
using CarGalleryHub.Application.DTOs.CarModel;

namespace CarGalleryHub.MVC.Models.DTOs.CarModel
{
    public class CarModelCreateView
    {
        public CarModelDataCreate CarModelData { get; set; } = new();

        public List<BrandListDto> Brands { get; set; } = new List<BrandListDto>();
    }
}
