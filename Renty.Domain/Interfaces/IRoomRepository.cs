using Renty.Domain.Models.Properties;
using Renty.Domain.Models.Properties.Anemities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Renty.Domain.Interfaces
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        Task<Room?> GetRoomWithDetailsAsync(Guid id, bool isActiveOnly = true, CancellationToken ct = default);

        Task<IEnumerable<Room>> GetRoomsByPropertyIdAsync(Guid propertyId, bool isActiveOnly = true, CancellationToken ct = default);

        // может там ремонт и она пока закрыта, но объект сдается
        Task<bool> ChangeStateAsync(Guid id, bool state, CancellationToken ct = default);

        //  Работа со типами комнат (RoomType) 

        Task<RoomType?> GetRoomTypeByIdAsync(Guid id, bool isActiveOnly = true, CancellationToken ct = default);

        Task<IEnumerable<RoomType>> GetAllRoomTypesAsync(bool isActiveOnly = true, CancellationToken ct = default);


    }
}
