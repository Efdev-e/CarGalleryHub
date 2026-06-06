using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Domain.Entities
{
    public class Brand : BaseEntity
    {
        public required string BrandName { get; set; }

        // FK

        public ICollection<CarModel> CarModels { get; set; } = new List<CarModel>();
    }
}
