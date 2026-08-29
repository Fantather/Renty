using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renty.Domain.Models.Properties;

namespace Renty.Infrastructure.Configurations.Properties
{
    public class PropertyTagConfiguration : IEntityTypeConfiguration<PropertyTag>
    {
        public void Configure(EntityTypeBuilder<PropertyTag> builder)
        {
            builder.ToTable("PropertyTags");

            builder.HasKey(pt => pt.Id);

            // Уникальная комбинация Property + Tag
            builder.HasIndex(pt => new { pt.PropertyId, pt.TagId })
                .IsUnique();

            builder.Property(pt => pt.CreatedAt)
                .IsRequired();

            // Связи
            builder.HasOne(pt => pt.Property)
                .WithMany(p => p.PropertyTags)
                .HasForeignKey(pt => pt.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pt => pt.Tag)
                .WithMany(t => t.PropertyTags)
                .HasForeignKey(pt => pt.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
