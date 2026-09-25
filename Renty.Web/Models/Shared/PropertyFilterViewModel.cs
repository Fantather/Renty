namespace Renty.Web.Models.Shared
{
    public class PropertyFilterViewModel
    {
        public string? CategorySlug { get; set; }

        // Поиск по городам
        public string? Destination { get; set; }

        // Даты заезда и выезда
        public DateOnly? CheckInDate { get; set; }
        public DateOnly? CheckOutDate { get; set; }

        // Гости — раздельно по категориям
        public int? AdultCount { get; set; }
        public int? ChildCount { get; set; }
        public int? InfantCount { get; set; }
        public int? PetCount { get; set; }

        // Границы видимой области карты
        public double? North { get; set; }
        public double? South { get; set; }
        public double? East { get; set; }
        public double? West { get; set; }
    }
}
