using System;
using System.Collections.Generic;
using System.Text;
using Renty.Domain.Models.Properties.Anemities;

namespace Renty.Domain.Interfaces
{
    public interface IAmenityRepository : IGenericRepository<Anemities>
    {
        // Получить все активные удобства
        Task<IEnumerable<Anemities>> GetAllActiveAsync(CancellationToken ct = default);

        // Получить все удобства, привязанные к конкретному объекту недвижимости
        Task<IEnumerable<Anemities>> GetAmenitiesByPropertyIdAsync(Guid propertyId, CancellationToken ct = default);

        // Получить все удобства, привязанные к конкретной комнате
        Task<IEnumerable<Anemities>> GetAmenitiesByRoomIdAsync(Guid roomId, CancellationToken ct = default);
    }
}
