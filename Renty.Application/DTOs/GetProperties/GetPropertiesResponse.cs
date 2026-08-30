using Renty.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetProperties
{
    public class GetPropertiesResponse
    ///  Модель для отображения главной страницы: списка недвижимости для аренды, категорий, 
    {
        public List<CategoryDto> Categories { get; set; } = new();

        public List<PropertyListItem> Properties { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    }
}
