using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetProperties
{
    [Obsolete("Заменён GetPropertiesResponse: границы карты теперь необязательные параметры GetPropertiesQuery")]
    public class GetPropertiesByMapResponse
    {
        public List<PropertyListItem> Properties { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
