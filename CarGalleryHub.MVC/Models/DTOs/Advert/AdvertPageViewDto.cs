using CarGalleryHub.Application.DTOs.Advert;

namespace CarGalleryHub.MVC.Models.DTOs.Advert
{
    public class AdvertPageViewDto
    {
        public int page { get; set; }
        public List<AdvertView> Dtos { get; set; } = new List<AdvertView>();
        public int totalCount { get; set; }
        public int pageSize { get; set; } = 9;
        public int TotalPages => (int)Math.Ceiling((double)totalCount / (pageSize > 0 ? pageSize : 9));
    }
}
