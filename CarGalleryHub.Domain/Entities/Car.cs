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
        public required string BrandName { get; set; } 
        public required string ModelName { get; set; }  
        public required string Series { get; set; }

        public string MotorPower { get; set; } = string.Empty;

        #endregion

        // --------------------------------------------

        #region Değişir

        public required int Year { get; set; }
        public required int KM { get; set; } 
        public required ColorType Color { get; set; }
        public required CarStatus Status { get; set; }
        public required CarAvailability Availability { get; set; }

        #endregion

        // ----- //
        public required int CarModelId { get; set; }

        // ----- //
        public CarModel CarModel { get; set; } = null!;
        public Advert Advert { get; set; } = null!;
    }
}
