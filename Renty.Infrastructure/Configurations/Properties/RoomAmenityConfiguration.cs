using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renty.Domain.Models.Properties;

namespace Renty.Infrastructure.Configurations.Properties
{
    /// <summary>
    /// Конфигурация сущности RoomAmenity через Fluent API
    /// </summary>
    public class RoomAmenityConfiguration : IEntityTypeConfiguration<RoomAmenity>
    {
        public void Configure(EntityTypeBuilder<RoomAmenity> builder)
        {
            // Название таблицы
            builder.ToTable("RoomAmenities");

            // Первичный ключ
            builder.HasKey(ra => ra.Id);

            // Уникальный индекс для предотвращения дублирования одного удобства в комнате
            builder.HasIndex(ra => new { ra.RoomId, ra.AmenityId })
                .IsUnique()
                .HasDatabaseName("IX_RoomAmenities_RoomId_AmenityId");

            builder.HasIndex(ra => ra.RoomId)
                .HasDatabaseName("IX_RoomAmenities_RoomId");

            builder.HasIndex(ra => ra.AmenityId)
                .HasDatabaseName("IX_RoomAmenities_AmenityId");

            // Свойства
            builder.Property(ra => ra.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(ra => ra.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Связи
            builder.HasOne(ra => ra.Room)
                .WithMany(r => r.RoomAmenities)
                .HasForeignKey(ra => ra.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ra => ra.Amenity)
                .WithMany()
                .HasForeignKey(ra => ra.AmenityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
