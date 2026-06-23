using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarGalleryHub.Domain.Entities
{
    public class Advert : BaseDateEntity
    {

        public  string AdvertTitle { get; set; } = string.Empty;
        public List<Image> Thumbnails { get; set; } = new List<Image>();
        public  string Description { get; set; } = string.Empty;

        public  bool Warranty { get; set; }
        public  decimal UnitPrice { get; set; }
        // ----- //
        public  int SellerId { get; set; }
        public  int CarId { get; set; }

        // ----- //
        public User Seller { get; set; } = null!;
        public Car Car { get; set; } = null!;

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public void DeleteIt() 
        {
            if (!IsDeleted)
                Delete();
        }
    }
}
