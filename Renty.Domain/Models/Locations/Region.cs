using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Renty.Domain.Models.Locations
{
    /// <summary>
    /// Регионы/Области
    /// </summary>
    public class Region
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        // Связь со страной
        public Guid CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; }

        // вместо удаления
        public bool IsActive { get; set; } = true;

        // Навигационные свойства
        public virtual ICollection<City> Cities { get; set; } = new List<City>();
    }
}
