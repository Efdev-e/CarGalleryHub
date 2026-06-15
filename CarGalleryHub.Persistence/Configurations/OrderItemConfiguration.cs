using CarGalleryHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Persistence.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();


            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.AdvertId).IsRequired();
            builder.Property(x => x.UnitPrice).IsRequired();
            builder.Property(x => x.CarYear).HasMaxLength(9999).IsRequired();
            builder.Property(x => x.CarKM).HasMaxLength(int.MaxValue).IsRequired();
            builder.Property(x => x.CarColor).IsRequired();
            builder.Property(x => x.BrandName).IsRequired();
            builder.Property(x => x.ModelName).IsRequired();
            builder.Property(x => x.OrderId).IsRequired();

            builder.HasOne(x => x.Advert)
                   .WithMany(x => x.OrderItems)
                   .HasForeignKey(x => x.AdvertId);

            builder.HasOne(x => x.Order)
                   .WithMany(x => x.OrderItems)
                   .HasForeignKey(x => x.OrderId);

            builder.HasOne(x => x.Thumbnail)
                   .WithOne(x => x.OrderItem)
                   .HasForeignKey<Image>(x => x.OrderItemId);
        }
    }
}
