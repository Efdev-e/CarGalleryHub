using CarGalleryHub.Application.Common.BaseDTOs;
using CarGalleryHub.Application.DTOs.Image;
using CarGalleryHub.Domain.Enum;

namespace CarGalleryHub.Application.DTOs.OrderItem
{
    public class OrderItemInfoDto : BaseDateEntityDto
    {
        public string ItemName { get; set; } = null!;
        public ImageDto? Thumbnail { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public required int Quantity { get; set; }

        public int CarYear { get; set; }
        public string CarKM { get; set; } = string.Empty;
        public ColorType CarColor { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;

        // ----- //
        public int? ImageId { get; set; }
        public required int AdvertId { get; set; }
        public required int OrderId { get; set; }

        // ----- //
    }
}
