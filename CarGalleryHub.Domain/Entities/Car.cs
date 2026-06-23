using CarGalleryHub.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Security.Principal;
using System.Text;

namespace CarGalleryHub.Domain.Entities
{
    public class Car : BaseDateEntity
    {

        // --------------------------------------------

        #region Değişmez
        public string BrandName { get; set; } = string.Empty; 
        public string ModelName { get; set; } = string.Empty;  
        public string Series { get; set; } = string.Empty;

        public string MotorPower { get; set; } = string.Empty;

        #endregion

        // --------------------------------------------

        #region Değişir

        public  int Year { get; set; }
        public  int KM { get; set; } 
        public  ColorType Color { get; set; }
        public  CarStatus Status { get; set; }
        public  CarAvailability Availability { get; set; }

        #endregion

        // ----- //
        public  int CarModelId { get; set; }

        // ----- //
        public List<int> UserIds { get; set; } = new List<int>();
        public CarModel CarModel { get; set; } = null!;
        public ICollection<Advert> Advert { get; set; } = null!;
    }
}
