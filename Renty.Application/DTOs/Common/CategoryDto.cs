using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.Common
{
    public class CategoryDto
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string Slug { get; set; } = null!;
        public string? ImageUrl { get; set; }
    }
}
