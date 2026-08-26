using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Renty.Domain.Models.Properties.Anemities
{
    /// <summary>
    /// Связь между комнатой и удобствами
    /// </summary>
    public class RoomAmenity
    {
        public Guid Id { get; set; }

        public Guid RoomId { get; set; }
        [ForeignKey(nameof(RoomId))]
        public virtual House Room { get; set; }

        public Guid AmenityId { get; set; }
        [ForeignKey(nameof(AmenityId))]
        public virtual Anemities Amenity { get; set; }

        // Активно ли удобство для данной комнаты
        public bool IsActive { get; set; } = true;
    }
}
