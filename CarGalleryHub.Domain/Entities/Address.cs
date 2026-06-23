using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Domain.Entities
{
    public class Address : BaseEntity
    {
        // Kişisel
        public  string FullName { get; set; } = string.Empty;
        public  string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        // Address
        public  string City { get; set; } = string.Empty;
        public  string District { get; set; } = string.Empty;
        public  string PostalCode { get; set; } = string.Empty;
        public string FullAddress { get; set; } = string.Empty;
        // FK
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
