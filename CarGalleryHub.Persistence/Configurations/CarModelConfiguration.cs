using CarGalleryHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Persistence.Configurations
{
    public class CarModelConfiguration : IEntityTypeConfiguration<CarModel>
    {
        public void Configure(EntityTypeBuilder<CarModel> builder)
        {
            builder.ToTable("CarModels");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.BrandId).IsRequired();

            builder.Property(x => x.Model).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Series).HasMaxLength(255).IsRequired();

            builder.HasOne(x => x.Brand)
                   .WithMany(x => x.CarModels)
                   .HasForeignKey(x => x.BrandId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.Cars)
                   .WithOne(x => x.CarModel)
                   .HasForeignKey(x => x.CarModelId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
