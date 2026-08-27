using System;
using System.Collections.Generic;

namespace Renty.Domain.Models.Properties
{
    /// <summary>
    /// Теги для категоризации и фильтрации недвижимости
    /// Например: "Pet-friendly", "Family-friendly", "Eco-friendly", "Luxury", etc.
    /// </summary>
    public class Tag
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        // ID иконки (может быть ссылкой на иконку или идентификатором из иконочного набора)
        public string? IconId { get; set; }

        // URL иконки (если используется прямая ссылка)
        public string? IconUrl { get; set; }

        // Свойство для определения порядка отображения тегов
        public int DisplayOrder { get; set; } = 0;

        // Описание тега
        public string? Description { get; set; }

        // Активность тега
        public bool IsActive { get; set; } = true;

        // Навигационные свойства
        public virtual ICollection<PropertyTag> PropertyTags { get; set; } = new List<PropertyTag>();
    }
}
