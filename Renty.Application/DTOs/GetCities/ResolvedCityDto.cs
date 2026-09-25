using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetCities
{
    public class ResolvedCityDto
    {
        public Guid CityId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
    }
}
