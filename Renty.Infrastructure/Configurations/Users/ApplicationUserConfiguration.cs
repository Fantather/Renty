using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renty.Domain.Models.User;

namespace Renty.Infrastructure.Configurations.Users
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("AspNetUsers");

            // Первичный ключ уже настроен Identity

            // Индексы
            builder.HasIndex(u => u.Email)
                .HasDatabaseName("IX_AspNetUsers_Email");

            builder.HasIndex(u => u.HomeCountryId)
                .HasDatabaseName("IX_AspNetUsers_HomeCountryId");

            builder.HasIndex(u => u.HomeCityId)
                .HasDatabaseName("IX_AspNetUsers_HomeCityId");

            builder.HasIndex(u => u.CreatedAt)
                .HasDatabaseName("IX_AspNetUsers_CreatedAt");

            // Свойства
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.AvatarUrl);

            builder.Property(u => u.TravelReason)
                .HasMaxLength(500);

            builder.Property(u => u.IsTravellingWithPet)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Связи с Country и City
            builder.HasOne(u => u.HomeCountry)
                .WithMany()
                .HasForeignKey(u => u.HomeCountryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(u => u.HomeCity)
                .WithMany()
                .HasForeignKey(u => u.HomeCityId)
                .OnDelete(DeleteBehavior.SetNull);

            // Связи с коллекциями
            builder.HasMany(u => u.Reviews)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Bookings)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Favorites)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
