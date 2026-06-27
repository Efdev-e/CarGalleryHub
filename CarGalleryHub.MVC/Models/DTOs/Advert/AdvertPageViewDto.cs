using CarGalleryHub.Application.DTOs.Advert;

namespace CarGalleryHub.MVC.Models.DTOs.Advert
{
    public class AdvertPageViewDto
    {
        public int page { get; set; }
        public List<AdvertView> Dtos { get; set; } = new List<AdvertView>();
    }
}
