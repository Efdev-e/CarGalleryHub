using CarGalleryHub.Application.DTOs.Address;
using CarGalleryHub.Application.DTOs.Cart;
using System.Collections.Generic;

namespace CarGalleryHub.MVC.Models
{
    public class CheckoutViewModel
    {
        // Saved Addresses for selection in the UI
        public List<AddressDto> SavedAddresses { get; set; } = new();

        // Cart details to display products and total amount
        public CartDto Cart { get; set; } = null!;

        // Order/Address Fields
        public string AddressFullName { get; set; } = string.Empty;
        public string AddressCity { get; set; } = string.Empty;
        public string AddressDistrict { get; set; } = string.Empty;
        public string AddressPostalCode { get; set; } = string.Empty;
        public string FullAddress { get; set; } = string.Empty;

        // Payment/Card Fields
        public string CardNumber { get; set; } = string.Empty;
        public string CardHolderName { get; set; } = string.Empty;
        public string ExpiryMonth { get; set; } = string.Empty;
        public string ExpiryYear { get; set; } = string.Empty;
        public string Cvv { get; set; } = string.Empty;
    }
}
