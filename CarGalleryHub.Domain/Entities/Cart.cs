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

        public int TotalAmount { get; private set; }
        public decimal TotalPrice { get; private set; }

        public Cart()
        {
            setTotalAmount();
            setTotalPrice();
        }

        public void setTotalPrice() 
        {
            TotalPrice = CartItems is null ? 1 : CartItems.Sum(x => x.UnitPrice * x.Quantity);
        }

        public void setTotalAmount() 
        {
            TotalAmount = CartItems is null ? 1 : CartItems.Sum(x => x.Quantity);
        }
    }
}
