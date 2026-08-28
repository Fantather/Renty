using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Renty.Domain.Models.Locations
{
    /// <summary>
    /// Города (используется для автокомплита при поиске)
    /// </summary>
    public class City
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();

        public string Name { get; set; } = string.Empty;

        // Связь с регионом
        public Guid? RegionId { get; set; }
        [ForeignKey(nameof(RegionId))]
        public virtual Region? Region { get; set; }

        // Связь со страной
        public Guid CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; }

        // Координаты центра города (для отображения на карте при поиске)
        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        // вместо удаления
        public bool IsActive { get; set; } = true;
    }
}
