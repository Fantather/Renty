using System;
using System.ComponentModel.DataAnnotations.Schema;
using Renty.Domain.Models.Properties.Anemities;

namespace Renty.Domain.Models.Properties
{
    /// <summary>
    /// Связь между комнатой и удобством
    /// Например: в спальне есть кондиционер, телевизор
    /// </summary>
    public class RoomAmenity
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();

        // Связь с комнатой
        public Guid RoomId { get; set; }
        [ForeignKey(nameof(RoomId))]
        public virtual Room Room { get; set; }

        // Связь с удобством
        public Guid AmenityId { get; set; }
        [ForeignKey(nameof(AmenityId))]
        public virtual Anemities.Anemities Amenity { get; set; }

        // Активно ли удобство
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
