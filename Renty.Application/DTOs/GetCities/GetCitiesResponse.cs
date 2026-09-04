using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetCities
{
    /// <summary>
    /// Список найденных городов по части строки
    /// </summary>
    public class GetCitiesResponse
    
    {
        public List<CityDto> Cities { get; set; } = new();
    }
}
