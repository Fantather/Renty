using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renty.Domain.Models.Properties;

namespace Renty.Infrastructure.Configurations.Properties
{
    /// <summary>
    /// Конфигурация сущности PropertyDetails через Fluent API
    /// </summary>
    public class PropertyDetailsConfiguration : IEntityTypeConfiguration<PropertyDetails>
    {
        public void Configure(EntityTypeBuilder<PropertyDetails> builder)
        {
            builder.ToTable("PropertyDetails");

            builder.HasKey(pd => pd.Id);

            // Свойства
            builder.Property(pd => pd.MaxGuests)
                .IsRequired();

            builder.Property(pd => pd.BedsCount)
                .IsRequired();

            builder.Property(pd => pd.BedroomsCount)
                .IsRequired();

            builder.Property(pd => pd.BathroomsCount)
                .IsRequired();

            builder.Property(pd => pd.FloorsCount)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(pd => pd.Floor)
                .IsRequired(false);
        }
    }
}
