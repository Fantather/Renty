using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Renty.Domain.Models.Properties.Anemities
{
    /// <summary>
    /// Удобства (Wi-Fi, кондиционер, парковка, бассейн и т.д.)
    /// </summary>
    public class Anemities
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty; // "Free Wi-Fi", "Air conditioning", "Pool", etc.

        public string? Description { get; set; }

        // Иконка удобства
        public string? IconUrl { get; set; }

        // Soft delete: активность удобства (true = активно, false = удалено)
        public bool IsActive { get; set; } = true;

        // Навигационные свойства
        public virtual ICollection<PropertyAmenity> PropertyAmenities { get; set; } = new List<PropertyAmenity>();
        public virtual ICollection<RoomAmenity> RoomAmenities { get; set; } = new List<RoomAmenity>();
    }
}
