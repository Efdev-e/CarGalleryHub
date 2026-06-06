using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Domain.Entities
{
    public class OrderItem : BaseDateEntity
    {
        public string ItemName => Advert is null ? string.Empty : Advert.AdvertTitle;
        public Image? Thumbnail { get; set; } = null!;
        public decimal UnitPrice => Advert is null ? 0 : Advert.UnitPrice;
        public required int Quantity { get; set; }

        // ----- //
        public int? ImageId { get; set; }
        public required int AdvertId { get; set; }
        public required int CartId { get; set; }

        // ----- //
        public Advert Advert { get; set; } = null!;
        public Cart Cart { get; set; } = null!;
    }
}
