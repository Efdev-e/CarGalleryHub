using CarGalleryHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarGalleryHub.Persistence.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("Images");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.ImageUrl).HasMaxLength(500);
            builder.Property(x => x.ImageType);
            builder.Property(x => x.ImageData);

            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Car)
                   .WithMany()
                   .HasForeignKey(x => x.CarId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Advert)
                   .WithMany()
                   .HasForeignKey(x => x.AdvertId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.OrderItem)
                   .WithMany()
                   .HasForeignKey(x => x.OrderItemId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CartItem)
                   .WithMany()
                   .HasForeignKey(x => x.CartItemId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Cart)
                   .WithMany()
                   .HasForeignKey(x => x.CartId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.ImageUrl);
        }
    }
}