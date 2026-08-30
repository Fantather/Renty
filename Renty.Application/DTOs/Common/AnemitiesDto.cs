using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.Common
{
    public class AnemitiesDto
    // Удобства (Wi-Fi, кондиционер, парковка, бассейн и т.д.)
    {
        public string Name { get; set; } = string.Empty; // "Free Wi-Fi", "Air conditioning", "Pool", etc.
        public string? Description { get; set; }
        public string? IconUrl { get; set; }
    }
}
