using Renty.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetProperty
{
    public class RoomDto
    // Модель для отображения комнат
    {
        // Название комнаты
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Является ли комната общего пользования
        // true = общая (гостиная, кухня, ванная общего пользования)
        // false = приватная (спальня, личная ванная)
        public bool IsSharedSpace { get; set; }

        // Количество спальных
        public int? BedsCount { get; set; }

        // Площадь комнаты
        public decimal? Area { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Удобства
        public List<AnemitiesDto> Anemities { get; set; } = new();

        // Тип комнаты (Studio, One Bedroom, Suite, Deluxe, etc.)
        public RoomTypeDto RoomType { get; set; } = null!;

        // Изображения комнаты
        public List<ImageDto> Images { get; set; } = new();

    }
}
