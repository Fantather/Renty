using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renty.Domain.Models.User;

namespace Renty.Infrastructure.Configurations.Users
{
    public class UserFactConfiguration : IEntityTypeConfiguration<UserFact>
    {
        public void Configure(EntityTypeBuilder<UserFact> builder)
        {
            builder.ToTable("UserFacts");

            builder.Property(f => f.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.Property(f => f.Value)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}
