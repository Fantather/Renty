using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renty.Domain.Models.Properties;

namespace Renty.Infrastructure.Configurations.Properties
{
    /// <summary>
    /// Конфигурация сущности Property через Fluent API
    /// </summary>
    public class PropertyConfiguration : IEntityTypeConfiguration<Property>
    {
        public void Configure(EntityTypeBuilder<Property> builder)
        {
            // Название таблицы
            builder.ToTable("Properties");

            // Первичный ключ
            builder.HasKey(p => p.Id);

            // Индексы
            builder.HasIndex(p => p.Slug).IsUnique();
            builder.HasIndex(p => p.HostId);
            builder.HasIndex(p => p.CityId);
            builder.HasIndex(p => p.CategoryId);
            
            //builder.HasIndex(p => p.IsActive);
            builder.HasIndex(p => p.PricePerNight);

            // Свойства
            builder.Property(p => p.Name)
                .IsRequired(false)
                .HasMaxLength(200);

            builder.Property(p => p.Slug)
                .IsRequired(false)
                .HasMaxLength(250);

            builder.Property(p => p.Description)
                .IsRequired(false)
                .HasMaxLength(5000);

            builder.Property(p => p.PricePerNight)
                .HasPrecision(18, 2);

            builder.Property(p => p.Currency)
                .IsRequired()
                .HasMaxLength(3); // ISO 4217 code

            builder.Property(p => p.AverageRating)
                .HasPrecision(3, 2);

            builder.Property(p => p.HouseRules)
                .HasMaxLength(2000);

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.UpdatedAt);

            // Связи (Foreign Keys)
            builder.HasOne(p => p.Host)
                .WithMany()
                .HasForeignKey(p => p.HostId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.HasOne(p => p.Address)
                .WithMany()
                .HasForeignKey(p => p.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.City)
                .WithMany()
                .HasForeignKey(p => p.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Country)
                .WithMany()
                .HasForeignKey(p => p.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Навигационные свойства
            builder.HasMany(p => p.Reviews)
                .WithOne(r => r.Property)
                .HasForeignKey(r => r.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Favorites)
                .WithOne(f => f.Property)
                .HasForeignKey(f => f.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.PropertyAmenities)
                .WithOne(pa => pa.Property)
                .HasForeignKey(pa => pa.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.PropertyImages)
                .WithOne(pi => pi.Property)
                .HasForeignKey(pi => pi.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.PropertyTags)
                .WithOne(pt => pt.Property)
                .HasForeignKey(pt => pt.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Bookings)
                .WithOne(b => b.Property)
                .HasForeignKey(b => b.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Details)
                .WithOne(d => d.Property)
                .HasForeignKey<PropertyDetails>(d => d.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Rooms)
                .WithOne(r => r.Property)
                .HasForeignKey(r => r.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(b => b.Status)
            .IsRequired()
            .HasConversion<string>() 
            .HasMaxLength(30);


            //// Игнорировать вычисляемые свойства
            //builder.Ignore(p => p.CurrentDetails);
        }
    }
}
