using CarGalleryHub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Domain.Entities
{
    public class Image : BaseEntity
    {
        public string ImageUrl { get; set; } = string.Empty;
        public ImageType ImageType { get; set; } = ImageType.Unknown;

        // FK

        public int? UserId { get; set; }
        public User? User { get; set; }

        public int? CarId { get; set; }
        public Car? Car { get; set; }

        public int? CartId { get; set; }
        public Cart? Cart { get; set; }
    }
}
