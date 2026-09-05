using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.Common
{
    /// <summary>
    /// Удобства (Wi-Fi, кондиционер, парковка, бассейн и т.д.)
    /// </summary>
    public class AmenitiesDto
    
    {
        public string Name { get; set; } = string.Empty; // "Free Wi-Fi", "Air conditioning", "Pool", etc.
        public string? Description { get; set; }
        public string? IconUrl { get; set; }
    }
}
