using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renty.Domain.Models.Locations;

namespace Renty.Infrastructure.Configurations.Locations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Countries");

            builder.HasKey(c => c.Id);

            builder.HasIndex(c => c.Name);
            builder.HasIndex(c => c.CountryCode).IsUnique();
            builder.HasIndex(c => c.IsActive);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.CountryCode)
                .IsRequired()
                .HasMaxLength(3); // ISO 3166-1 alpha-2 or alpha-3

            builder.Property(c => c.CurrencyCode)
                .HasMaxLength(3); // ISO 4217

            builder.Property(c => c.PhoneCode)
                .HasMaxLength(10);

            builder.Property(c => c.IsActive)
                .IsRequired();

            builder.HasMany(c => c.Cities)
                .WithOne(city => city.Country)
                .HasForeignKey(city => city.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
