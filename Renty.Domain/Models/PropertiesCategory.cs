using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Renty.Domain.Models.Properties;

namespace Renty.Domain.Models
{
    /// <summary>
    /// Категории объектов недвижимости (Apartment, House, Villa, Hotel, Guesthouse, Hostel, etc.)
    /// </summary>
    public class PropertiesCategory
    {


        // Навигационные свойства
        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
