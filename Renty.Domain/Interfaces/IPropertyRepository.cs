using System;
using System.Collections.Generic;
using System.Text;
using Renty.Domain.Models.Properties;
using Renty.Domain.Parameters;

namespace Renty.Domain.Interfaces
{
   public interface IPropertyRepository : IGenericRepository<Property>
    {
        // Получить объект недвижимости с деталями (включая связанные сущности)
        Task<Property?> GetPropertyWithDetailsAsync(Guid id, CancellationToken ct = default);
        Task<Property?> GetPropertyWithDetailsAsync(string slug, CancellationToken ct = default);

        // Для главной страницы (каталога) с возможностью фильтрации
        Task<IEnumerable<Property>> GetPropertiesForCatalogAsync(ParametersPropertiesForCatalog param, CancellationToken ct = default
        );

        //получить объекты недвижимости по имени пользователя владельца или айди владельца
        Task<IEnumerable<Property>> GetPropertiesByHostAsync(Guid? hostId = null, string? username = null, CancellationToken ct = default);
    }
}
