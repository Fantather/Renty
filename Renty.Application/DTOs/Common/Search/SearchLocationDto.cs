using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.Common.Search
{
    public class SearchLocationDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string FormattedAddress { get; set; } = string.Empty;
    }
}
