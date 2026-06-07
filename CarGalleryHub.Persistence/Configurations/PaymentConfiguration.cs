using CarGalleryHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Amount).HasPrecision(18,2).IsRequired();
            builder.Property(x => x.PaymentStatus).IsRequired();

            builder.Property(x => x.OrderId).IsRequired();

            builder.HasOne(x => x.Order)
                   .WithOne(x => x.Payment)
                   .HasForeignKey<Order>(x => x.PaymentId);
        }
    }
}
