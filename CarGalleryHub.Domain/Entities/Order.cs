using CarGalleryHub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Domain.Entities
{
    public class Order : BaseDateEntity
    {
        // ----- //
        public string OrderNumber { get; set; } = string.Empty;
        public OrderStatus OrderStatus { get; set; }
        // ----- //
        public  string UserFullName { get; set; } = string.Empty;
        public  string UserPhone { get; set; } = string.Empty;
        public  string UserEmail { get; set; } = string.Empty;
        public decimal TotalCost { get; private set; }


        // ----- //

        public  string AddressFullName { get; set; } = string.Empty;
        public  string AddressCity { get; set; } = string.Empty;
        public  string AddressDistrict { get; set; } = string.Empty;
        public  string AddressPostalCode { get; set; } = string.Empty;
        public  string FullAddress { get; set; } = string.Empty;

        // ----- //
        public  int UserId { get; init; }
        // ----- //
        public ICollection<OrderItem> OrderItems { get; init; } = new List<OrderItem>();
        public Payment? Payment { get; set; } = null!;
        public User User { get; set; } = null!;

        public Order()
        {
            EnsureTotalCost();
            SetCreatedAt();
        }

        public void EnsureTotalCost()
        {
            TotalCost = OrderItems is null ? 0 : OrderItems.Sum(x => x.UnitPrice * x.Quantity);
        }

    }
}
