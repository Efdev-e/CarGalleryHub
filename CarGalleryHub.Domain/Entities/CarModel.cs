using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Domain.Entities
{
    public class CarModel : BaseEntity
    {
        // Araba Mdl
        public string BrandName { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Series { get; set; } = string.Empty;

        public DateTime ReleaseDate { get; set; }

        // FK
        public required int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;
        public ICollection<Car> Cars { get; set; } = new List<Car>();
    }
}
