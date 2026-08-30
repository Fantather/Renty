using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.Common
{
    public class ImageDto
    // Модель для отображения изображений
    {
        // Путь к изображению
        public string ImageUrl { get; set; } = string.Empty;

        //// Название изображения
        //public string Title { get; set; } =string.Empty!;

        //// Описание изображения
        //public string? Description { get; set; }

        // Порядок отображеения
        public int DisplayOrder { get; set; }

        // Является ли изображение главным
        public bool IsPrimary { get; set; }
        // Дата загрузки
        public DateTime CreatedAt { get; set; }

    }
}
