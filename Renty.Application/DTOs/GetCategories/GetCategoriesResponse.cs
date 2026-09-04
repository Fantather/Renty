using Renty.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetCategories
{
    /// <summary>
    /// Список категорий для главной страницы
    /// </summary>
    public class GetCategoriesResponse
    
    {
        public List<CategoryDto> Categories { get; set; } = new();

    }
}
