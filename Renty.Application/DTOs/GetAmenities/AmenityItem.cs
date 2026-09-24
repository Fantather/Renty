using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetAmenities
{
    public class AmenityItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string IconName { get; set; } = null!;
    }
}
