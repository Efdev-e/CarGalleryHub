using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.Order
{
    public class CreateOrderDto
    {
        public required string AddressFullName { get; set; }
        public required string AddressCity { get; set; }
        public required string AddressDistrict { get; set; }
        public required string AddressPostalCode { get; set; }
        public required string FullAddress { get; set; }
    }
}
