using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renty.Domain.Models;

namespace Renty.Infrastructure.Configurations.Properties
{
    public class PropertiesCategoryConfiguration : IEntityTypeConfiguration<PropertiesCategory>
    {
        public void Configure(EntityTypeBuilder<PropertiesCategory> builder)
        {
            builder.ToTable("PropertiesCategories");

            builder.HasKey(pc => pc.Id);

            // Индексы
            builder.HasIndex(pc => pc.Name)
                .HasDatabaseName("IX_PropertiesCategories_Name");

            builder.HasIndex(pc => pc.IsActive)
                .HasDatabaseName("IX_PropertiesCategories_IsActive");

            // Свойства (унаследованные от Category)
            builder.Property(pc => pc.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(pc => pc.Description)
                .HasMaxLength(500);

            builder.Property(pc => pc.IconUrl)
                .HasMaxLength(500);

            builder.Property(pc => pc.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(pc => pc.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(pc => pc.UpdatedAt)
                .IsRequired(false);

            // Связи
            builder.HasMany(pc => pc.Properties)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
