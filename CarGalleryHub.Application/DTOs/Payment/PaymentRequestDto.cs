using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.Payment
{
    public class PaymentRequestDto
    {
        public int OrderId { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public string CardHolderNumner { get; set; } = string.Empty;
        public string ExpiryMonth { get; set; } = string.Empty;
        public string ExpiryYear { get; set; } = string.Empty;
        public string Cvv { get; set; } = string.Empty;
    }
}
