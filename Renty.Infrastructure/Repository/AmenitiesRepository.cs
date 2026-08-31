
using Microsoft.EntityFrameworkCore;
using Renty.Domain.Interfaces;
using Renty.Infrastructure.Data;
using Renty.Domain.Models.Properties.Anemities;

namespace Renty.Infrastructure.Repository
{
    public class AmenitiesRepository : GenericRepository<Anemities>, IAmenityRepository
    {
        public AmenitiesRepository(AppDbContext context) : base(context)
        {
        }
        /// <summary>
        /// Изменяет состояние активности удобства(как тупо это звучит) по его идентификатору. Если удобство активно, оно станет неактивным, и наоборот.
        /// </summary>
        /// <param name="id">Идентификатор удобства.</param>
        /// <param name="ct">Токен отмены для асинхронной операции.</param>
        /// <returns>True, если состояние было успешно изменено; иначе false.</returns>
        public async Task<bool> ChangeStateAsync(Guid id, CancellationToken ct = default)
        {
            var amenity = await _dbSet.FirstOrDefaultAsync(a => a.Id == id, ct);
            if (amenity == null)
            {
                return false;
            }
            amenity.IsActive = !amenity.IsActive; //переключение состояния активности
            await _context.SaveChangesAsync(ct);
            return true;

        }
        /// <summary>
        /// Возвращает все удобства, с возможностью фильтрации по активности.
        /// </summary>
        /// <param name="activeOnly">Если true, возвращаются только активные удобства.</param>
        /// <param name="ct">Токен отмены для асинхронной операции.</param>
        /// <returns>Список удобств.</returns>
        public async Task<IEnumerable<Anemities>> GetAllAsync(bool activeOnly = true, CancellationToken ct = default)
        {
            var query = _dbSet.AsNoTracking();
            if (activeOnly)
            {
                query = query.Where(a => a.IsActive);
            }

            return await query.ToListAsync(ct);
        }
        /// <summary>
        /// Возвращает все удобства, связанные с конкретным объектом недвижимости по его идентификатору.
        /// </summary>
        /// <param name="propertyId">Идентификатор объекта недвижимости.</param>
        /// <param name="activeOnly">Если true, возвращаются только активные удобства.</param>
        /// <param name="ct">Токен отмены для асинхронной операции.</param>
        /// <returns>Список удобств.</returns>
        public async Task<IEnumerable<Anemities>> GetAmenitiesByPropertyIdAsync(Guid propertyId, bool activeOnly = true, CancellationToken ct = default)
        {
            var query = _dbSet.Where(a => a.PropertyAmenities.Any(pa => pa.PropertyId == propertyId));
            if (activeOnly)
            {
                query = query.Where(a => a.IsActive);
            }
            return await query.AsNoTracking().ToListAsync(ct);
        }
        
        /// <summary>
        /// Возвращает все удобства, связанные с конкретной комнатой по ее идентификатору.
        /// </summary>
        /// <param name="roomId">Идентификатор комнаты.</param>
        /// <param name="activeOnly">Если true, возвращаются только активные удобства.</param>
        /// <param name="ct">Токен отмены для асинхронной операции.</param>
        /// <returns>Список удобств.</returns>
        public async Task<IEnumerable<Anemities>> GetAmenitiesByRoomIdAsync(Guid roomId, bool activeOnly = true, CancellationToken ct = default)
        {
            var query = _dbSet.Where(a => a.RoomAmenities.Any(ra => ra.RoomId == roomId));
            if (activeOnly)
            {
                query = query.Where(a => a.IsActive);
            }
            return await query.AsNoTracking().ToListAsync(ct);
        }

    }
}
