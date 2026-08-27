using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renty.Domain.Models.Properties;

namespace Renty.Infrastructure.Configurations.Properties
{
    /// <summary>
    /// Конфигурация сущности Room через Fluent API
    /// </summary>
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            // Название таблицы
            builder.ToTable("Rooms");

            // Первичный ключ
            builder.HasKey(r => r.Id);

            // Индексы
            builder.HasIndex(r => r.PropertyId)
                .HasDatabaseName("IX_Rooms_PropertyId");

            builder.HasIndex(r => new { r.PropertyId, r.IsActive })
                .HasDatabaseName("IX_Rooms_PropertyId_IsActive");

            builder.HasIndex(r => new { r.PropertyId, r.DisplayOrder })
                .HasDatabaseName("IX_Rooms_PropertyId_DisplayOrder");

            // Свойства
            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(r => r.Description)
                .HasMaxLength(1000);

            builder.Property(r => r.IsSharedSpace)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(r => r.BedsCount)
                .IsRequired(false);

            builder.Property(r => r.Area)
                .HasPrecision(18, 2)
                .IsRequired(false);

            builder.Property(r => r.DisplayOrder)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(r => r.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(r => r.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(r => r.UpdatedAt)
                .IsRequired(false);

            // Связи
            builder.HasOne(r => r.Property)
                .WithMany(p => p.Rooms)
                .HasForeignKey(r => r.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.Images)
                .WithOne(ri => ri.Room)
                .HasForeignKey(ri => ri.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.RoomAmenities)
                .WithOne(ra => ra.Room)
                .HasForeignKey(ra => ra.RoomId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
