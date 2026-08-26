
using System.ComponentModel.DataAnnotations.Schema;
using Renty.Domain.Models;


namespace Renty.Domain.Models.Properties.Anemities
{
    /// <summary>
    /// Связь между отелем/недвижимостью и удобствами
    /// </summary>
    public class HotelAmenity
    {
        public Guid Id { get; set; }

        public Guid PropertyId { get; set; }
        [ForeignKey(nameof(PropertyId))]
        public virtual Property Property { get; set; }

        public int AmenityId { get; set; }
        [ForeignKey(nameof(AmenityId))]
        public virtual Anemities Amenity { get; set; }

        public bool IsActive { get; set; } = true;

    }
}
