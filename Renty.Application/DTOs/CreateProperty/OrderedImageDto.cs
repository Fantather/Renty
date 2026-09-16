using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.CreateProperty
{
    public class OrderedImageDto
    {
        public Guid ImageId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
    }
}
