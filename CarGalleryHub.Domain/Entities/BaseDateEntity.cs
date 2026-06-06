using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Domain.Entities
{
    public abstract class BaseDateEntity : BaseEntity
    {
        public DateTime CreatedAt { get; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; }

        protected void Updated() => UpdatedAt = DateTime.UtcNow;
    }
}
