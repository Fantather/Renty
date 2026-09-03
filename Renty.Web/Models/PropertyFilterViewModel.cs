namespace Renty.Web.Models
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
    }
}
