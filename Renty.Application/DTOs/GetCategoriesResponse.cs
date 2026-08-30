using Renty.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs
{
    public class GetCategoriesResponse
    // Список категорий для главной страницы
    {
        public List<CategoryDto> Categories { get; set; } = new();

    }
}
