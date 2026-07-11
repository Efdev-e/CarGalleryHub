using CarGalleryHub.Application.DTOs.Brand;

namespace CarGalleryHub.MVC.Models.DTOs.Brand
{
    public class BrandViewDto
    {
        public int page { get; set; }
        public List<BrandListDto> Dtos { get; set; } = new List<BrandListDto>();
    }
}
