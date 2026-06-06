using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Domain.Entities
{
    public class Address : BaseEntity
    {
        // Kişisel
        public required string FullName { get; set; }
        public required string Phone { get; set; }
        public string Email { get; set; } = string.Empty;
        // Address
        public required string City { get; set; }
        public required string District { get; set; } 
        public required string PostalCode { get; set; } 
        public required string FullAddress { get; set; }
        // FK
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
