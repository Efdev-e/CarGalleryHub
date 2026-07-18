using CarGalleryHub.Application.DTOs.Brand;

namespace CarGalleryHub.MVC.Models.DTOs.Brand
{
    public class BrandViewDto
    {
        public int page { get; set; }
        public List<BrandListDto> Dtos { get; set; } = new List<BrandListDto>();
        public int totalCount { get; set; }
        public int pageSize { get; set; } = 9;
        public int TotalPages => (int)Math.Ceiling((double)totalCount / (pageSize > 0 ? pageSize : 9));
    }
}
