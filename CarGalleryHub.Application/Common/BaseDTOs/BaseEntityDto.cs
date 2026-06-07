using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarGalleryHub.Application.Common.BaseDTOs
{
    public abstract class BaseEntityDto
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int Id { get; set; }
    }
}
