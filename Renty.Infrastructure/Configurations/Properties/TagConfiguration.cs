using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renty.Domain.Models.Properties;

namespace Renty.Infrastructure.Configurations.Properties
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable("Tags");

            builder.HasKey(t => t.Id);

            builder.HasIndex(t => t.DisplayOrder);
            builder.HasIndex(t => t.IsActive);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.IconId)
                .HasMaxLength(50);

            builder.Property(t => t.IconUrl);

            builder.Property(t => t.Description)
                .HasMaxLength(500);

            builder.Property(t => t.IsActive)
                .IsRequired();

            builder.HasMany(t => t.PropertyTags)
                .WithOne(pt => pt.Tag)
                .HasForeignKey(pt => pt.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
