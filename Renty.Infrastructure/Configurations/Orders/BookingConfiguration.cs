using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renty.Domain.Models.Orders;

namespace Renty.Infrastructure.Configurations.Orders
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Bookings");

            builder.HasKey(b => b.Id);

            builder.HasIndex(b => b.PropertyId);
            builder.HasIndex(b => b.UserId);
            builder.HasIndex(b => b.StatusId);
            builder.HasIndex(b => new { b.CheckInDate, b.CheckOutDate });
            builder.HasIndex(b => b.CreatedAt);

            builder.Property(b => b.CheckInDate)
                .IsRequired();

            builder.Property(b => b.CheckOutDate)
                .IsRequired();

            builder.Property(b => b.GuestsCount)
                .IsRequired();

            builder.Property(b => b.TotalPrice)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(b => b.CreatedAt)
                .IsRequired();

            // Связи
            builder.HasOne(b => b.Property)
                .WithMany(p => p.Bookings)
                .HasForeignKey(b => b.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Status)
                .WithMany()
                .HasForeignKey(b => b.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
