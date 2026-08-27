using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Renty.Domain.Models.Locations
{
    /// <summary>
    /// Города (используется для автокомплита при поиске)
    /// </summary>
    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        // Связь с регионом
        public int? RegionId { get; set; }
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
