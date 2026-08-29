using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Renty.Domain.Models.Properties.Anemities
{
    /// <summary>
    /// Связь между недвижимостью и удобствами
    /// </summary>
    public class PropertyAmenity
    {
        public Guid Id { get; set; }

        public Guid PropertyId { get; set; }
        [ForeignKey(nameof(PropertyId))]
        public virtual Property Property { get; set; }

        public Guid AmenityId { get; set; }
        [ForeignKey(nameof(AmenityId))]
        public virtual Anemities Amenity { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
