using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renty.Domain.Models.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Infrastructure.Configurations.Locations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

            builder.HasKey(r => r.Id);

            builder.HasIndex(a => a.CityId);
            builder.HasIndex(a => a.PlaceId).IsUnique();
            builder.HasIndex(c => c.Location)
               .HasMethod("gist");

            builder.Property(c => c.FullAddress)
                .HasMaxLength(300);

            builder.Property(c => c.Street)
               .HasMaxLength(200);

            builder.Property(c => c.District)
                .HasMaxLength(200);

            builder.Property(c => c.Location)
              .HasColumnType("geometry(Point, 4326)");

            // Связи
            builder.HasOne(a => a.City)
                .WithMany(c => c.Addresses)
                .HasForeignKey(a => a.CityId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
