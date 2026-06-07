using CarGalleryHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Persistence.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName).HasMaxLength(50).IsRequired(true);
            builder.Property(x => x.LastName).HasMaxLength(50).IsRequired(true);

            builder.Property(x => x.Email).HasMaxLength(255).IsRequired(true);
            builder.Property(x => x.PhoneNumber).HasMaxLength(20);
            builder.Property(x => x.PasswordHash).IsRequired(true);

            builder.HasMany(x => x.Addresses)
                   .WithMany(x => x.Users);

            builder.HasMany(x => x.Orders)
                   .WithOne(x => x.User)
                   .HasForeignKey(x => x.UserId);

            builder.HasMany(x => x.Carts)
                   .WithOne(x => x.User)
                   .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.ProfilePicture)
                   .WithOne(x => x.User)
                   .HasForeignKey<Image>(x => x.UserId);

        }
    }
}
