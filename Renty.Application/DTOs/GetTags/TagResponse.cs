using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetTags
{
    public class TagResponse
    {
        public Guid Id { get; set; }
        public string? IconName { get; set; }
        public string Name { get; set; } = null!;
    }
}
