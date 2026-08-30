using Renty.Application.DTOs.Common;
using Renty.Application.DTOs.GetProperties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetProperty
{
    public class GetPropertyDetailsResponse
        // Детали об аренде 
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public CategoryDto Category { get; set; } = null!;

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
        public string Status { get; set; } = null!;

        // Изображеня комнат
        public List<ImageDto> Images { get; set; } = new();

        // Диапазон свободных дат для заселения или что то такое
        //public DateTime 

        // Дата создания объявления и последнего обновления
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string Tag { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Адрес, область и местоположение
        public string? District { get; set; }
        public string? Street { get; set; }
        public string Address { get; set; } = string.Empty;
        // Координаты местополложения на карте
        //public Point? Point { get; set; }

        // Характеристики
        public int MaxGuests { get; set; }
        public int BedsCount { get; set; }
        public int BedroomsCount { get; set; }
        public int BathroomsCount { get; set; } 
        
        // Количество комнат
        public int RoomsCount { get; set; }

        // Количество этажей
        public int FloorsCount { get; set; }
        // Этаж
        public int? Floor { get; set; }

        // Комнаты
        public List<RoomDto> Rooms { get; set; } = new();

        // Правила 
        public string? HouseRules { get; set; }

        // Времена заезда/выезда
        public TimeSpan? CheckInTime { get; set; }
        public TimeSpan? CheckOutTime { get; set; }


        // Владелец 
        public HostDto Host { get; set; } = null!;

        // Удобства
        public List<AnemitiesDto> Anemities { get; set; } = new();
    }
}
