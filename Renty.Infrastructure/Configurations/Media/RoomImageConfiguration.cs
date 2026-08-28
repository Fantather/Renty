using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renty.Domain.Models.Properties;

namespace Renty.Infrastructure.Configurations.Properties
{
    /// <summary>
    /// Конфигурация сущности RoomImage через Fluent API
    /// </summary>
    public class RoomImageConfiguration : IEntityTypeConfiguration<RoomImage>
    {
        public void Configure(EntityTypeBuilder<RoomImage> builder)
        {
            // Название таблицы
            builder.ToTable("RoomImages");

            // Первичный ключ
            builder.HasKey(ri => ri.Id);

            // Индексы
            builder.HasIndex(ri => ri.RoomId)
                .HasDatabaseName("IX_RoomImages_RoomId");

            builder.HasIndex(ri => new { ri.RoomId, ri.IsPrimary })
                .HasDatabaseName("IX_RoomImages_RoomId_IsPrimary");

            //builder.HasIndex(ri => new { ri.RoomId, ri.DisplayOrder })
            //    .HasDatabaseName("IX_RoomImages_RoomId_DisplayOrder");

            // Свойства из базового класса Image
            builder.Property(ri => ri.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(ri => ri.Title)
                .IsRequired()
                .HasMaxLength(200)
                .HasDefaultValue("Title Image");

            builder.Property(ri => ri.Description)
                .HasMaxLength(500);

            //builder.Property(ri => ri.DisplayOrder)
            //    .IsRequired()
            //    .HasDefaultValue(0);

            builder.Property(ri => ri.IsPrimary)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(ri => ri.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            // Связи
            builder.HasOne(ri => ri.Room)
                .WithMany(r => r.Images)
                .HasForeignKey(ri => ri.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            //builder.HasOne(ri => ri.ImageType)
            //    .WithMany()
            //    .HasForeignKey(ri => ri.ImageTypeId)
            //    .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
