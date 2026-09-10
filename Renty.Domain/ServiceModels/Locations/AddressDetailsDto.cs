using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.ServiceModels.Locations
{
    public class AddressDetailsDto
    {
        public string FormattedAddress { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public string RegionName { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
