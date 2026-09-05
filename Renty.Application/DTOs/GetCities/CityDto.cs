using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetCities
{
    /// <summary>
    /// Отображение города при поиске с его айди, областью и страной 
    /// </summary>
    public class CityDto
    
    {
        public Guid CityId { get; set; }
        public string CityName { get; set; } = null!;
        public string? RegionName { get; set; }
        public string CountryName { get; set; } = null!;
    }
}
