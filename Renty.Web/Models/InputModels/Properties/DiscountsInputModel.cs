using System.ComponentModel.DataAnnotations;

namespace Renty.Web.Models.InputModels.Properties
{
    /// <summary>
    /// Скидки, предлагаемые гостям при создании объявления
    /// </summary>
    public class DiscountsInputModel
    {
        // скидка на первые бронирования нового объявления
        [Range(0, 100)]
        public int NewListingDiscountPercent { get; set; } = 20;
        public bool NewListingDiscountEnabled { get; set; } = true;

        // скидка при бронировании прямо перед заездом
        [Range(0, 100)]
        public int LastMinuteDiscountPercent { get; set; } = 6;
        public bool LastMinuteDiscountEnabled { get; set; } = true;

        // скидка при поездке от недели
        [Range(0, 100)]
        public int WeeklyDiscountPercent { get; set; } = 10;
        public bool WeeklyDiscountEnabled { get; set; } = true;

        // скидка при поездке от месяца
        [Range(0, 100)]
        public int MonthlyDiscountPercent { get; set; } = 20;
        public bool MonthlyDiscountEnabled { get; set; } = true;
    }
}
