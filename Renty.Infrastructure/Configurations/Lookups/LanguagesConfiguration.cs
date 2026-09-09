using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renty.Domain.Models.LookupsTables;

namespace Renty.Infrastructure.Configurations.Lookups
{
    public class LanguagesConfiguration : IEntityTypeConfiguration<Languages>
    {
        public void Configure(EntityTypeBuilder<Languages> builder)
        {
            builder.ToTable("Languages");

            builder.HasKey(l => l.Id);

            builder.HasIndex(l => l.Code).IsUnique();

            builder.Property(l => l.Code)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(l => l.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(l => l.Users)
                .WithMany(u => u.Languages)
                .UsingEntity(j => j.ToTable("UserLanguages"));
        }
    }
}