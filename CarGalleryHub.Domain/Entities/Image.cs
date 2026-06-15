using CarGalleryHub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Domain.Entities
{
    public class Image : BaseEntity
    {
        public required string ImageUrl { get; set; } = string.Empty;
        public required ImageType ImageType { get; set; } = ImageType.Unknown;

        // FK

        public int? UserId { get; set; }
        public User? User { get; set; }

        public int? CarId { get; set; }
        public Car? Car { get; set; }

        public int AdvertId { get; set; }
        public Advert? Advert { get; set; }

        public int? OrderItemId { get; set; }
        public OrderItem? OrderItem { get; set; }

        public int? CartItemId { get; set; }
        public CartItem? CartItem { get; set; }

        public int? CartId { get; set; }
        public Cart? Cart { get; set; }
    }
}
