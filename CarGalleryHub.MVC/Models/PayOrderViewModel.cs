using CarGalleryHub.Application.DTOs.Order;

namespace CarGalleryHub.MVC.Models
{
    public class PayOrderViewModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public decimal TotalCost { get; set; }

        // Payment/Card Fields
        public string CardNumber { get; set; } = string.Empty;
        public string CardHolderName { get; set; } = string.Empty;
        public string ExpiryMonth { get; set; } = string.Empty;
        public string ExpiryYear { get; set; } = string.Empty;
        public string Cvv { get; set; } = string.Empty;
    }
}
