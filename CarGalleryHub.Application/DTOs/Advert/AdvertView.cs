using CarGalleryHub.Application.DTOs.Image;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.Advert
{
    public class AdvertView
    {
        public int Id { get; set; }
        public required string AdvertTitle { get; set; }
        public string? ImageUrl { get; set; }
        public required string Description { get; set; }

        public required decimal UnitPrice { get; set; }
    }
}
