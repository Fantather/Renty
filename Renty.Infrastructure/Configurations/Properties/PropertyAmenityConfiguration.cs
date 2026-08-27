using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renty.Domain.Models.Properties.Anemities;

namespace Renty.Infrastructure.Configurations.Properties
{
    public class PropertyAmenityConfiguration : IEntityTypeConfiguration<PropertyAmenity>
    {
        public void Configure(EntityTypeBuilder<PropertyAmenity> builder)
        {
            builder.ToTable("PropertyAmenities");

            builder.HasKey(pa => pa.Id);

            // Уникальная комбинация Property + Amenity
            builder.HasIndex(pa => new { pa.PropertyId, pa.AmenityId })
                .IsUnique();

            builder.HasIndex(pa => pa.IsActive);

            builder.Property(pa => pa.IsActive)
                .IsRequired();

            // Связи
            builder.HasOne(pa => pa.Property)
                .WithMany(p => p.PropertyAmenities)
                .HasForeignKey(pa => pa.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pa => pa.Amenity)
                .WithMany(a => a.PropertyAmenities)
                .HasForeignKey(pa => pa.AmenityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
