using CarGalleryHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasIndex(x => x.OrderNumber).IsUnique(true);

            builder.Property(x => x.OrderNumber).IsRequired(true);

            builder.Property(x => x.UserFullName).IsRequired();
            builder.Property(x => x.UserPhone).IsRequired();
            builder.Property(x => x.UserEmail).IsRequired();

            builder.Property(x => x.AddressFullName).IsRequired();
            builder.Property(x => x.AddressCity).IsRequired();
            builder.Property(x => x.AddressDistrict).IsRequired();
            builder.Property(x => x.AddressPostalCode).IsRequired();
            builder.Property(x => x.AddressFullName).IsRequired();

            builder.Property(x => x.UserId).IsRequired();

            builder.HasMany(x => x.OrderItems)
                   .WithOne(x => x.Order)
                   .HasForeignKey(x => x.OrderId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.Orders)
                   .HasForeignKey(x => x.UserId);
        }
    }
}
