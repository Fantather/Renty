using Renty.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetProperties
{
    /// <summary>
    /// Модель для отображения списка недвижимости для аренды на главной странице
    /// </summary>
    public class GetPropertiesResponse
    
    {

        public List<PropertyListItem> Properties { get; set; } = new();

        public int TotalCount { get; set; }

        // Текущая страница
        public int Page { get; set; } = 1;

        // Количество объектов на странице
        public int PageSize { get; set; } = 5;

        // Всего страниц
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    }
}
