using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renty.Domain.Models.User;

namespace Renty.Infrastructure.Configurations.Users
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("Reviews");

            builder.HasKey(r => r.Id);

            builder.HasIndex(r => r.PropertyId);
            builder.HasIndex(r => r.UserId);
            builder.HasIndex(r => r.CreatedAt);

            builder.Property(r => r.Rating)
                .IsRequired()
                .HasPrecision(3, 2);

            builder.Property(r => r.CleanlinessRating)
                .HasPrecision(3, 2);

            builder.Property(r => r.CommunicationRating)
                .HasPrecision(3, 2);

            builder.Property(r => r.AccuracyRating)
                .HasPrecision(3, 2);

            builder.Property(r => r.LocationRating)
                .HasPrecision(3, 2);

            builder.Property(r => r.Comment)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(r => r.HostResponse)
                .HasMaxLength(2000);

            builder.Property(r => r.CreatedAt)
                .IsRequired();

            // Связи
            builder.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Property)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
