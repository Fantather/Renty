using Microsoft.EntityFrameworkCore;
using Renty.Domain.Models.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Renty.Infrastructure.Configurations.Properties
{
    public class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
    {
        public void Configure(EntityTypeBuilder<RoomType> builder)
        {
            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(rt => rt.Description)
                .HasMaxLength(500);

            builder.Property(rt => rt.IsActive)
                .HasDefaultValue(true);

            //builder.Property(rt => rt.DisplayOrder)
            //    .HasDefaultValue(0);
        }
    }
}
