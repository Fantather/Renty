namespace Renty.Web.Models.Shared
{
    public class PropertyCardViewModel
    {
        public Guid Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string? ActionUrl { get; set; }
        public List<string> ImageUrls { get; set; } = new();
        public bool IsFavorite { get; set; }
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public decimal Rating { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string DurationLabel { get; set; } = string.Empty;
        // Итоговая цена за ночь, уже с учётом скидки
        public decimal PricePerNight { get; set; }
        // Цена до скидки, выводится зачёркнутой; null, если скидки нет
        public decimal? OriginalPricePerNight { get; set; }
        // Подпись скидки под ценой, например «−10% · новое объявление»; null, если скидки нет
        public string? DiscountLabel { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
