using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.Parameters
{
    public sealed class ParametersPropertiesForCatalog
    {
        public int Skip { get; set; }
        public int PageSize { get; set; }
        public Guid? CityId { get; set; } = null;
        public Guid? CategoryId { get; set; } = null;
        public string? CategorySlug { get; set; } = null;
        public string? SortBy { get; set; } = null;
        public DateTime? CheckInDate { get; set; } = null;
        public DateTime? CheckOutDate { get; set; } = null;
        public int? GuestCount { get; set; } = null;
    }
}
