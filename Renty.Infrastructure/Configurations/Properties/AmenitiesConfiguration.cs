using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renty.Domain.Models.Properties.Anemities;

namespace Renty.Infrastructure.Configurations.Properties
{
    public class AmenitiesConfiguration : IEntityTypeConfiguration<Anemities>
    {
        public void Configure(EntityTypeBuilder<Anemities> builder)
        {
            builder.ToTable("Amenities");

            builder.HasKey(a => a.Id);

            builder.HasIndex(a => a.IsActive);

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.Description)
                .HasMaxLength(500);

            builder.Property(a => a.IconUrl)
                .HasMaxLength(500);

            builder.Property(a => a.IsActive)
                .IsRequired();

            builder.HasMany(a => a.PropertyAmenities)
                .WithOne(pa => pa.Amenity)
                .HasForeignKey(pa => pa.AmenityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.RoomAmenities)
                .WithOne(ra => ra.Amenity)
                .HasForeignKey(ra => ra.AmenityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
