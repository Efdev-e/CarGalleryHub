using System;
using System.Collections.Generic;
using System.Text;
using CarGalleryHub.Domain.Enum;

namespace CarGalleryHub.Domain.Entities
{
    public class OrderItem : BaseDateEntity
    {
        public string ItemName {get; set;} = null!;
        public Image? Thumbnail { get; set; } = null!;
        public decimal UnitPrice { get; set;}
        public required int Quantity { get; set; }

        public int CarYear { get; set; }
        public string CarKM { get; set; } = string.Empty;
        public ColorType CarColor { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public string ModelName { get; set; } = string.Empty;

        // ----- //
        public int? ImageId { get; set; }
        public int? AdvertId { get; set; }

        // ----- //
        public Advert? Advert { get; set; }
    }
}
