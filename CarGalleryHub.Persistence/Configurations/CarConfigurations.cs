using CarGalleryHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Persistence.Configurations
{
    public class CarConfigurations : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.ToTable("Cars");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.BrandName).HasMaxLength(255).IsRequired();
            builder.Property(x => x.ModelName).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Series).HasMaxLength(255).IsRequired();

            builder.Property(x => x.MotorPower).HasMaxLength(255);

            builder.Property(x => x.Year).IsRequired();
            builder.Property(x => x.KM).HasMaxLength(int.MaxValue).IsRequired();
            builder.Property(x => x.Color).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.Availability).IsRequired();

            builder.Property(x => x.CarModelId).IsRequired();
            builder.HasOne(x => x.CarModel)
                   .WithMany(x => x.Cars)
                   .HasForeignKey(x => x.CarModelId);

        }
    }
}
