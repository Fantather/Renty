using System;
using System.Collections.Generic;
using System.Text;
using Renty.Domain.Models.Properties;

namespace Renty.Domain.Interfaces
{
   public interface IPropertyRepository : IGenericRepository<Property>
    {
        // Получить объект недвижимости с деталями (включая связанные сущности)
        Task<Property?> GetPropertyWithDetailsAsync(Guid id);
        Task<Property?> GetPropertyWithDetailsAsync(string slug);

        // Для главной страницы (каталога) с возможностью фильтрации
        Task<IEnumerable<Property>> GetPropertiesForCatalogAsync(Guid? cityId = null, Guid? categoryId = null, string? categorySlug = null);

        //получить объекты недвижимости по имени пользователя владельца или айди владельца
        Task<IEnumerable<Property>> GetPropertiesByHostAsync(Guid? hostId = null, string? username = null);
    }
}
