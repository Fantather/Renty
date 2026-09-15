using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Renty.Application.DTOs.CreateProperty
{
    public class DiscountsInputDto
    {
        // скидка на первые бронирования нового объявления
        public int NewListingDiscountPercent { get; set; } 
        public bool NewListingDiscountEnabled { get; set; } 

        // скидка при бронировании прямо перед заездом
        public int LastMinuteDiscountPercent { get; set; } 
        public bool LastMinuteDiscountEnabled { get; set; } 

        // скидка при поездке от недели
        public int WeeklyDiscountPercent { get; set; }
        public bool WeeklyDiscountEnabled { get; set; } 

        // скидка при поездке от месяца
        public int MonthlyDiscountPercent { get; set; } 
        public bool MonthlyDiscountEnabled { get; set; } 
    }
}
