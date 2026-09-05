using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetProperty
{
    /// <summary>
    /// Отображение тега
    /// </summary>
    public class TagDto
    
    {
        // ID иконки (может быть ссылкой на иконку или идентификатором из иконочного набора)
        public Guid? IconId { get; set; }

        // URL иконки (если используется прямая ссылка)
        public Guid? IconUrl { get; set; }
        public string Name { get; set; } = null!;
    }
}
