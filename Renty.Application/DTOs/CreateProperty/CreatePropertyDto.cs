using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.CreateProperty
{
    public class CreatePropertyDto
    {
        public string Title { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public Guid HostId { get; set; }

        // поля гугл мапс
        public string RawAddress { get; set; } = string.Empty;
        public string? Street { get; set; }
        public string? District { get; set; }
        public string? CityName { get; set; }
        public string? CountryName { get; set; }
        public string? CountryCode { get; set; }


        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
