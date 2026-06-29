using CarGalleryHub.Application.DTOs.CarModel;
using CarGalleryHub.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace CarGalleryHub.MVC.Models.DTOs.Car
{
    public class CarFormViewDto
    {
        public int Id { get; set; }
        public List<CarModelPageData> Models { get; set; } = new();
        public CarFormData Car { get; set; } = new();
    }

    public class CarFormData
    {
        [Required]
        public int CarModelId { get; set; }

        [Required]
        [Range(1900, 2100)]
        public int Year { get; set; } = DateTime.UtcNow.Year;

        [Required]
        [Range(0, int.MaxValue)]
        public int KM { get; set; }

        public string MotorPower { get; set; } = string.Empty;

        [Required]
        public ColorType Color { get; set; }

        [Required]
        public CarStatus Status { get; set; }

        [Required]
        public CarAvailability Availability { get; set; }
    }
}
