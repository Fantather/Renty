using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.ServiceModels.Places
{
    public class CitySuggestionDto
    {
        public string PlaceId { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
    }
}
