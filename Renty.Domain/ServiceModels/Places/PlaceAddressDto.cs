using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.ServiceModels.Places
{
    public class PlaceAddressDto
    {
        public string? StreetName { get; set; }
        public string? StreetNumber { get; set; }
        public string? District { get; set; }
        public string? CityName { get; set; }
        public string? CountryName { get; set; }
    }
}
