using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Domain.Entities
{
    public class Cart : BaseDateEntity
    {
        //
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public required int UserId { get; set; }
        public User User { get; set; } = null!;

        public int TotalAmount => CartItems.Sum(x => x.Quantity);
        public decimal TotalPrice => CartItems.Sum(x => x.UnitPrice * x.Quantity);
    }
}
