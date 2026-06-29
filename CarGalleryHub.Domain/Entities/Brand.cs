using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Domain.Entities
{
    public class Brand : BaseEntity
    {
        public string BrandName { get; set; } = string.Empty;

        // FK

        public ICollection<CarModel> CarModels { get; set; } = new List<CarModel>();

        public void DeleteBrand() 
        {
            if (IsDeleted == false) 
            {
                this.Delete();
            }
        }
    }
}
