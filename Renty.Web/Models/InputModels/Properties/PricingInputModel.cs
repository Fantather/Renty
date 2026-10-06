using System.ComponentModel.DataAnnotations;

namespace Renty.Web.Models.InputModels.Properties
{
    public class PricingInputModel
    {
        [Required(ErrorMessage = "Укажите цену за ночь")]
        [Range(1, 1000000, ErrorMessage = "Цена должна быть больше нуля")]
        public decimal PricePerNight { get; set; }

        public string Currency { get; set; } = "USD";

        // наценка на пятницу/субботу в процентах ("Коэффициент выходных")
        [Range(0, 100, ErrorMessage = "Наценка должна быть от 0 до 100%")]
        public int? WeekendPricePercent { get; set; }
    }
}
