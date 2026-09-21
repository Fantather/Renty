using Renty.Domain.Interfaces;
using Renty.Domain.Models.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Renty.Domain.Models
{
    /// <summary>
    /// Категории родитель
    /// </summary>
    public abstract class Category: IHasSlug
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();

        public string Slug { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        // URL картинки категории
        public string? ImageUrl { get; set; }

        // ID иконки (может быть ссылкой на иконку или идентификатором из иконочного набора)
        public Guid? IconId { get; set; }

        // URL иконки (если используется прямая ссылка)
        public string? IconUrl { get; set; }

        // Активна ли категория
        public bool IsActive { get; set; } = true;

        // Даты
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
