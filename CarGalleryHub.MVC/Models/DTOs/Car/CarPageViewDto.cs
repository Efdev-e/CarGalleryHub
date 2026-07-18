using CarGalleryHub.Application.DTOs.Car;

namespace CarGalleryHub.MVC.Models.DTOs.Car
{
    public class CarPageViewDto
    {
        public int page { get; set; }
        public List<CarInfoDto> Dtos { get; set; } = new();
        public int totalCount { get; set; }
        public int pageSize { get; set; } = 9;
        public int TotalPages => (int)Math.Ceiling((double)totalCount / (pageSize > 0 ? pageSize : 9));
    }
}
