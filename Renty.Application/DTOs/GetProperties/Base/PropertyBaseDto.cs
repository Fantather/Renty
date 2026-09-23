using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetProperties.Base
{
    public abstract class PropertyBaseDto
    {
        public string Slug { get; set; } = null!;
        public string CategoryName { get; set; } = null!;

        // Добавленна ли недвижимость в список избранных

        public bool IsFavorite { get; set; }

        // Страна, город
        public string CityName { get; set; } = null!;
        public string CountryName { get; set; } = null!;

        //nullable что бы не сломать если что
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        // В зависимости от выбранной страны пользователя (по умолчанию в USD)
        public decimal PricePerNight { get; set; }
        // Рейтинг
        public decimal AverageRating { get; set; }

        // Титульное изображение
        public string CoverImage { get; set; } = null!;
    }
}
