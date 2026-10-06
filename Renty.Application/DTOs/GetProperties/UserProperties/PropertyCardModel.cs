using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetProperties.UserProperties
{
    public class PropertyCardModel
    {
        public Guid Id { get; set; }
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public decimal Rating { get; set; }
        public decimal PricePerNight { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
