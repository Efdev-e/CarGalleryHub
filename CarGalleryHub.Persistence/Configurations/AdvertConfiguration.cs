using CarGalleryHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Persistence.Configurations
{
    public class AdvertConfiguration : IEntityTypeConfiguration<Advert>
    {
        public void Configure(EntityTypeBuilder<Advert> builder)
        {
            builder.ToTable("Adverts");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.AdvertTitle).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500).IsRequired();
            builder.Property(x => x.Warranty).IsRequired();
            builder.Property(x => x.UnitPrice).IsRequired();
            builder.Property(x => x.SellerId).IsRequired();
            builder.Property(x => x.CarId).IsRequired();

            builder.HasOne(x => x.Seller)
                   .WithMany(x => x.Adverts)
                   .HasForeignKey(x => x.SellerId);

            builder.HasOne(x => x.Car)
                   .WithOne(x => x.Advert)
                   .HasForeignKey<Advert>(x => x.CarId);

            builder.HasMany(x => x.CartItems)
                   .WithOne(x => x.Advert)
                   .HasForeignKey(x => x.AdvertId);

            builder.HasMany(x => x.OrderItems)
                   .WithOne(x => x.Advert)
                   .HasForeignKey(x => x.AdvertId);

            builder.HasMany(x => x.Thumbnails)
                   .WithOne(x => x.Advert)
                   .HasForeignKey(x => x.AdvertId);
        }
    }
}
