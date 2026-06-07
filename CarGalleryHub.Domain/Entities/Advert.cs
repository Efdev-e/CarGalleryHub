using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarGalleryHub.Domain.Entities
{
    public class Advert : BaseDateEntity
    {

        public required string AdvertTitle { get; set; } = string.Empty;
        public List<Image> Thumbnails { get; set; } = new List<Image>();
        public required string Description { get; set; } = string.Empty;

        public required bool Warranty { get; set; }
        public required decimal UnitPrice { get; set; }
        // ----- //
        public required int SellerId { get; set; }
        public required int CarId { get; set; }

        // ----- //
        public User Seller { get; set; } = null!;
        public Car Car { get; set; } = null!;

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
