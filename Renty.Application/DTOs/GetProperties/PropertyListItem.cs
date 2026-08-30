using Renty.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetProperties
{
    public class PropertyListItem
    ///  Модель для отображения недвижимости для аренды
    {
        public string Slug { get; set; } = null!;

        public string Name { get; set; } = string.Empty;
        //public string Tag { get; set; } = string.Empty;
        public string CategoryName { get; set; } = null!;

        // Добавленна ли недвижимость в список избранных
        public bool IsFavorite { get; set; }

        // Страна город
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;

        // В зависимости от выбранной страны пользователя (по умолчанию в USD)
        public decimal PricePerNight { get; set; }
        public string Currency { get; set; } = null!;

        // Рейтинг и отзывы
        public decimal AverageRating { get; set; }
        public int ReviewsCount { get; set; }

        // Статус
        //[Description("В ожидании")]
        //Pending = 1,
        //[Description("Завершено")]
        //Completed = 2,
        //[Description("Не удалось")]
        //Failed = 3,
        //[Description("Возвращено")]
        //Refunded = 4
        public string Status { get; set; } = null!;

        // Титульное изображение
        public string CoverImage { get; set; } = null!;

        // Продолжительность заселения (19 дек. - 5 янв.) 
        public string Duration { get; set; } = null!;

        // Дата создания объявления и обновления
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
