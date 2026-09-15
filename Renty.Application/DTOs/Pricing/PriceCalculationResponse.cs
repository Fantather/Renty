using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.Pricing
{
    public class PriceCalculationResponse
    {
        public int Nights { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal BaseTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalTotal { get; set; }
    }
}
