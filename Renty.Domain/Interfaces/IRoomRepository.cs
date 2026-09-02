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
        /// <summary>
        /// Получение полной информации о комнате по идентификатору, с возможностью фильтрации по активности
        /// </summary>
        /// <param name="id">Идентификатор комнаты</param>
        /// <param name="isActiveOnly">Флаг, указывающий, нужно ли возвращать только активные комнаты</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Комната с полной информацией или null, если не найдена</returns>
        Task<Room?> GetRoomWithDetailsAsync(Guid id, bool isActiveOnly = true, CancellationToken ct = default);
        /// <summary>
        /// Получение всех комнат по идентификатору объекта недвижимости, с возможностью фильтрации по активности
        /// </summary>
        /// <param name="propertyId">Идентификатор объекта недвижимости</param>
        /// <param name="isActiveOnly">Флаг, указывающий, нужно ли возвращать только активные комнаты</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Список комнат</returns>
        Task<IEnumerable<Room>> GetRoomsByPropertyIdAsync(Guid propertyId, bool isActiveOnly = true, CancellationToken ct = default);

        /// <summary>
        /// Мягкое удаление или возвращение активности комнаты по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор комнаты</param>
        /// <param name="state">Новое состояние активности комнаты</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Флаг, указывающий, было ли изменение состояния успешным</returns>
        Task<bool> ChangeStateAsync(Guid id, bool state, CancellationToken ct = default);


        /// <summary>
        ///  Получение типа комнаты по идентификатору, с возможностью фильтрации по активности
        /// </summary>
        /// <param name="id">Идентификатор типа комнаты</param>
        /// <param name="isActiveOnly">Флаг, указывающий, нужно ли возвращать только активные типы комнат</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Тип комнаты или null, если не найден</returns>
        Task<RoomType?> GetRoomTypeByIdAsync(Guid id, bool isActiveOnly = true, CancellationToken ct = default);
        /// <summary>
        /// Получение всех комнат по идентификатору объекта недвижимости, с возможностью фильтрации по активности
        /// </summary>
        /// <param name="propertyId">Идентификатор объекта недвижимости</param>
        /// <param name="isActiveOnly">Флаг, указывающий, нужно ли возвращать только активные комнаты</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Список комнат</returns>
        Task<IEnumerable<RoomType>> GetAllRoomTypesAsync(bool isActiveOnly = true, CancellationToken ct = default);


    }
}
