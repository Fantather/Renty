using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Renty.Domain.Models.Locations
{
    /// <summary>
    /// Страны
    /// </summary>
    public class Country
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        // Код страны ISO (US, GB, RU, etc.)
        public string CountryCode { get; set; } = string.Empty;

        // Код валюты (USD, GBP, RUB, etc.)
        public string? CurrencyCode { get; set; }

        // Телефонный код страны
        public string? PhoneCode { get; set; }

        // вместо удаления
        public bool IsActive { get; set; } = true;

        // Навигационные свойства
        public virtual ICollection<City> Cities { get; set; } = new List<City>();
    }
}
