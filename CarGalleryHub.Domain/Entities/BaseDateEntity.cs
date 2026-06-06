using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Domain.Entities
{
    public abstract class BaseDateEntity : BaseEntity
    {
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        protected void Updated() => UpdatedAt = DateTime.UtcNow;
    }
}
