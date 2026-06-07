using CarGalleryHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Persistence.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FullName).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Phone).HasMaxLength(20).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(255);
            builder.Property(x => x.City).HasMaxLength(255).IsRequired();
            builder.Property(x => x.District).HasMaxLength(255).IsRequired();
            builder.Property(x => x.FullName).HasMaxLength(500).IsRequired();
            builder.Property(x => x.PostalCode).HasMaxLength(7).IsRequired();


            builder.HasMany(x => x.Users)
                   .WithMany(x => x.Addresses);


        }
    }
}
