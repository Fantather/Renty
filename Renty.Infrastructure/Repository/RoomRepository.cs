using Microsoft.EntityFrameworkCore;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.Properties;
using Renty.Infrastructure.Data;


namespace Renty.Infrastructure.Repository
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Мягкое удаление или возвращение активности комнаты по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор комнаты</param>
        /// <param name="state">Новое состояние активности комнаты</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Флаг, указывающий, было ли изменение состояния успешным</returns>
        public async Task<bool> ChangeStateAsync(Guid id, bool state, CancellationToken ct = default)
        {
            var room = await _dbSet.FirstOrDefaultAsync(r => r.Id == id, ct);
            if (room == null)
            {
                return false;
            }

            room.IsActive = state;
            room.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);
            return true;
        }
        /// <summary>
        /// Получение всех типов комнат, с возможностью фильтрации по активности
        /// </summary>
        /// <param name="isActiveOnly">Флаг, указывающий, нужно ли возвращать только активные типы комнат</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Список типов комнат</returns>
        public async Task<IEnumerable<RoomType>> GetAllRoomTypesAsync(bool isActiveOnly = true, CancellationToken ct = default)
        {
            var query = _context.Set<RoomType>().AsQueryable();

            if (isActiveOnly)
            {
                query = query.Where(rt => rt.IsActive);
            }

            return await query.AsNoTracking().ToListAsync(ct);
        }
        /// <summary>
        /// Получение всех комнат по идентификатору объекта недвижимости, с возможностью фильтрации по активности
        /// </summary>
        /// <param name="propertyId">Идентификатор объекта недвижимости</param>
        /// <param name="isActiveOnly">Флаг, указывающий, нужно ли возвращать только активные комнаты</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Список комнат</returns>
        public async Task<IEnumerable<Room>> GetRoomsByPropertyIdAsync(Guid propertyId, bool isActiveOnly = true, CancellationToken ct = default)
        {
            var query = _dbSet
                .Where(r => r.PropertyId == propertyId)
                .Include(r => r.RoomType)
                .Include(r => r.Images)
                .Include(r => r.RoomAmenities)
                 .ThenInclude(ra => ra.Amenity)
                .AsQueryable();

            if (isActiveOnly)
            {
                query = query.Where(r => r.IsActive);
            }

            return await query.AsNoTracking().ToListAsync(ct);
        }
        /// <summary>
        ///  Получение типа комнаты по идентификатору, с возможностью фильтрации по активности
        /// </summary>
        /// <param name="id">Идентификатор типа комнаты</param>
        /// <param name="isActiveOnly">Флаг, указывающий, нужно ли возвращать только активные типы комнат</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Тип комнаты или null, если не найден</returns>
        public async Task<RoomType?> GetRoomTypeByIdAsync(Guid id, bool isActiveOnly = true, CancellationToken ct = default)
        {
            var query = _context.Set<RoomType>().AsQueryable();

            if (isActiveOnly)
            {
                query = query.Where(rt => rt.IsActive);
            }

            return await query.AsNoTracking().FirstOrDefaultAsync(rt => rt.Id == id, ct);
        }
        /// <summary>
        /// Получение комнаты с деталями по идентификатору, с возможностью фильтрации по активности
        /// </summary>
        /// <param name="id">Идентификатор комнаты</param>
        /// <param name="isActiveOnly">Флаг, указывающий, нужно ли возвращать только активные комнаты</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Комната с деталями или null, если не найдена</returns>

        public async Task<Room?> GetRoomWithDetailsAsync(Guid id, bool isActiveOnly = true, CancellationToken ct = default)
        {
            var query = _dbSet
                .Include(r => r.RoomType)
                .Include(r => r.Images)
                .Include(r => r.RoomAmenities)
                 .ThenInclude(ra => ra.Amenity)
                .AsQueryable();

            if (isActiveOnly)
            {
                query = query.Where(r => r.IsActive);
            }

            return await query.FirstOrDefaultAsync(r => r.Id == id, ct);
        }
    }
}
