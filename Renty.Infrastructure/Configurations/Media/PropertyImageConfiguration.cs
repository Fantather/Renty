using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renty.Domain.Models.Media;

namespace Renty.Infrastructure.Configurations.Media
{
    public class PropertyImageConfiguration : IEntityTypeConfiguration<PropertyImage>
    {
        public void Configure(EntityTypeBuilder<PropertyImage> builder)
        {
            builder.ToTable("PropertyImages");

            builder.HasKey(pi => pi.Id);

            builder.HasIndex(pi => pi.PropertyId);
            builder.HasIndex(pi => pi.ImageTypeId);

            builder.Property(pi => pi.ImageUrl)
                .IsRequired();

            builder.Property(pi => pi.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(pi => pi.Description)
                .HasMaxLength(500);

            builder.Property(pi => pi.IsPrimary)
                .IsRequired();

            builder.Property(pi => pi.DisplayOrder)
                .IsRequired();

            builder.Property(pi => pi.CreatedAt)
                .IsRequired();

            // Связи
            builder.HasOne(pi => pi.Property)
                .WithMany(p => p.PropertyImages)
                .HasForeignKey(pi => pi.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pi => pi.ImageType)
                .WithMany()
                .HasForeignKey(pi => pi.ImageTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
