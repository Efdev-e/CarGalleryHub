using CarGalleryHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarGalleryHub.Persistence.Configurations
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.AdvertId).IsRequired();
            builder.Property(x => x.CartId).IsRequired();

            builder.HasOne(x => x.Advert)
                   .WithMany(x => x.CartItems)
                   .HasForeignKey(x => x.AdvertId);

            builder.HasOne(x => x.Cart)
                   .WithMany(x => x.CartItems)
                   .HasForeignKey(x => x.CartId);

            builder.HasOne(x => x.Thumbnail)
                   .WithOne(x => x.CartItem)
                   .HasForeignKey<CartItem>(x => x.ImageId);

        }
    }
}
