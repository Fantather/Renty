using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetProperties.UserProperties
{
    /// <summary>
    /// Модель со всеми объявлениями пользователя
    /// </summary>
    public class GetUserPropertiesResponse
    {
        public List<PropertyCardModel> Drafts { get; set; } = new();
        public List<PropertyCardModel> Published { get; set; } = new();
    }
}
