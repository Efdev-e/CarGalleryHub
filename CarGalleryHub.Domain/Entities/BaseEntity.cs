using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; private set; }

        protected void Delete() => IsDeleted = true;
        protected void UnDelete() => IsDeleted = false;
    }
}
