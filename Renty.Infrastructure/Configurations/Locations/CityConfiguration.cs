using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renty.Domain.Models.Locations;

namespace Renty.Infrastructure.Configurations.Locations
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("Cities");

            builder.HasKey(c => c.Id);

            builder.HasIndex(c => c.Name);
            builder.HasIndex(c => c.CountryId);
            builder.HasIndex(c => c.RegionId);
            builder.HasIndex(c => new { c.Latitude, c.Longitude });
            builder.HasIndex(c => c.IsActive);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.Latitude)
                .HasPrecision(10, 7);

            builder.Property(c => c.Longitude)
                .HasPrecision(10, 7);

            builder.Property(c => c.IsActive)
                .IsRequired();

            // Связи
            builder.HasOne(c => c.Country)
                .WithMany(country => country.Cities)
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Region)
                .WithMany(r => r.Cities)
                .HasForeignKey(c => c.RegionId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
