namespace Renty.Web.Models.Properties
{
    // Средние оценки по категориям, посчитанные по всем отзывам квартиры
    public class RatingBreakdownViewModel
    {
        public decimal Cleanliness { get; set; }
        public decimal Accuracy { get; set; }
        public decimal CheckIn { get; set; }
        public decimal Communication { get; set; }
        public decimal Location { get; set; }
        public decimal Value { get; set; }
    }
}
