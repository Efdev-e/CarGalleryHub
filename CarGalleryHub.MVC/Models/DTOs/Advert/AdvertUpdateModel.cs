using CarGalleryHub.Application.DTOs.Image;

namespace CarGalleryHub.MVC.Models.DTOs.Advert
{
    public class AdvertUpdateModel
    {
        public int Id { get; set; }
        public string? AdvertTitle { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public decimal? UnitPrice { get; set; }
        // ----- //
        public int? CarId { get; set; }
    }
}
