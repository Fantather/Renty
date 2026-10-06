using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetProperties
{
    public class GetPropertiesByMapResponse
    {
        public List<PropertyListItem> Properties { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
