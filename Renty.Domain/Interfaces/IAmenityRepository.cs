using System;
using System.Collections.Generic;
using System.Text;
using Renty.Domain.Models.Properties.Anemities;

namespace Renty.Domain.Interfaces
{
    public interface IAmenityRepository : IGenericRepository<Anemities>
    {
        // Получить все активные удобства
        Task<IEnumerable<Anemities>> GetAllAsync(bool activeOnly = true, CancellationToken ct = default);

        // Получить все удобства, привязанные к конкретному объекту недвижимости
        Task<IEnumerable<Anemities>> GetAmenitiesByPropertyIdAsync(Guid propertyId, bool activeOnly = true, CancellationToken ct = default);

        // Получить все удобства, привязанные к конкретной комнате
        Task<IEnumerable<Anemities>> GetAmenitiesByRoomIdAsync(Guid roomId, bool activeOnly = true, CancellationToken ct = default);

        // Изменить состояние удобства (активно/неактивно)(soft delete)
        Task<bool> ChangeStateAsync(Guid id, CancellationToken ct = default);


    }
}
