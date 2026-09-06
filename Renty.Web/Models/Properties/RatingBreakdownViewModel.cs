namespace Renty.Web.Models.Properties
{
    // Средние оценки по категориям, посчитанные по всем отзывам квартиры
    public class RatingBreakdownViewModel
    {
        public decimal Cleanliness { get; set; } // чистота
        public decimal Accuracy { get; set; } // соответствие описанию/фото
        public decimal CheckIn { get; set; } // удобство заезда
        public decimal Communication { get; set; } // общение с хозяином
        public decimal Location { get; set; } // расположение
        public decimal Value { get; set; } // соотношение цена/качество
    }
}
