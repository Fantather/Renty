using Renty.Domain.Models.LookupsTables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetProperty
{
    public class DiscountDto
    {
        public DiscountTypeEnum Type { get; set; }
        public decimal Percentage { get; set; }
        public int? DaysBeforeCheckIn { get; set; }
        public int? MinNights { get; set; }
    }
}
