using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renty.Domain.Models.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Infrastructure.Configurations
{

    public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.ToTable("Discounts");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Percentage)
                .HasColumnType("decimal(5, 2)")
                .HasDefaultValue(20.00m)
                .IsRequired();

            builder.Property(d => d.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Property(d => d.CurrentUses)
                .HasDefaultValue(null);

            builder.Property(d => d.IsActive)
                .HasDefaultValue(true);

            builder.HasOne(d => d.Property)
                .WithMany(p => p.Discounts)
                .HasForeignKey(d => d.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

