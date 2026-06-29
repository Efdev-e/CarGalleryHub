using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Application.DTOs.CarModel
{
    public class CarModelDataCreate
    {
        public string Series { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public int BrandId { get; set; }
    }
}
