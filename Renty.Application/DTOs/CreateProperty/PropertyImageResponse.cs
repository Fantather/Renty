using MimeKit.Tnef;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.CreateProperty
{
    public class PropertyImageResponse
    {
        public Guid PropertyId { get; set; }
        public List<string> ImageUrls { get; set; } = new();
        public List<Guid> ImageIds { get; set; } = new();
    }
}
