using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Renty.Domain.Models.Media
{
    public class UpdateImageMetadataRequest
    {
        [Required]
        public Guid PropertyId { get; set; }

        public List<ImageMetadataItem> Images { get; set; } = new();
    }


}
