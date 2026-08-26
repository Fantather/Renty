using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Renty.Domain.Models.Properties;

namespace Renty.Domain.Models
{
    /// <summary>
    /// Категории родитель
    /// </summary>
    public abstract class Category
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        // URL иконки категории
        public string? IconUrl { get; set; }

        // Активна ли категория
        public bool IsActive { get; set; } = true;

        // Даты
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
