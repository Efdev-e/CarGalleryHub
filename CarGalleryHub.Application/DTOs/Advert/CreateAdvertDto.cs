using CarGalleryHub.Application.DTOs.Image;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.Advert
{
    public class CreateAdvertDto
    {
        public required string AdvertTitle { get; set; }
        public required string ImageUrl { get; set; }
        public required string Description { get; set; }
        public required bool Warranty { get; set; } = false;
        // ----- //
        public int SellerId { get; set; }
        public required int CarId { get; set; }

        // ----- //

        // ----- //

        public required decimal UnitPrice { get; set; }
    }
}
