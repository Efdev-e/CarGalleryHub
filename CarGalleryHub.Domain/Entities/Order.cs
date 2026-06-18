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
        public required string UserFullName { get; set; }
        public required string UserPhone { get; set; }
        public required string UserEmail { get; set; }
        public decimal TotalCost { get; private set; }


        // ----- //

        public required string AddressFullName { get; set; }
        public required string AddressCity { get; set; }
        public required string AddressDistrict { get; set; }
        public required string AddressPostalCode { get; set; }
        public required string FullAddress { get; set; }

        // ----- //
        public required int UserId { get; init; }
        // ----- //
        public ICollection<OrderItem> OrderItems { get; init; } = new List<OrderItem>();
        public Payment? Payment { get; set; } = null!;
        public User User { get; set; } = null!;

        public Order()
        {
            EnsureTotalCost();
        }

        public void EnsureTotalCost()
        {
            TotalCost = OrderItems is null ? 0 : OrderItems.Sum(x => x.UnitPrice * x.Quantity);
        }

    }
}
