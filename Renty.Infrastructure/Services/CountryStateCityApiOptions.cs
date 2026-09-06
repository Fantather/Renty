using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Infrastructure.Services
{
    public class CountryStateCityApiOptions
    {
        public const string SectionName = "Countrystatecity-api";
        public string BASE_URL { get; set; } = string.Empty;
        public string X_CSCAPI_KEY { get; set; } = string.Empty;
    }
}
