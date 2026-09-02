using Microsoft.EntityFrameworkCore;
using Renty.Domain.Models.Properties.Anemities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.Interfaces
{
    public interface IAmenityRepository : IGenericRepository<Anemities>
    {
        /// <summary>
        /// Возвращает все удобства, с возможностью фильтрации по активности.
        /// </summary>
        /// <param name="activeOnly">Если true, возвращаются только активные удобства.</param>
        /// <param name="ct">Токен отмены для асинхронной операции.</param>
        /// <returns>Список удобств.</returns>
        Task<IEnumerable<Anemities>> GetAllAsync(bool activeOnly = true, CancellationToken ct = default);

        /// <summary>
        /// Возвращает все удобства, связанные с конкретным объектом недвижимости по его идентификатору.
        /// </summary>
        /// <param name="propertyId">Идентификатор объекта недвижимости.</param>
        /// <param name="activeOnly">Если true, возвращаются только активные удобства.</param>
        /// <param name="ct">Токен отмены для асинхронной операции.</param>
        /// <returns>Список удобств.</returns>
        Task<IEnumerable<Anemities>> GetAmenitiesByPropertyIdAsync(Guid propertyId, bool activeOnly = true, CancellationToken ct = default);


        /// <summary>
        /// Возвращает все удобства, связанные с конкретной комнатой по ее идентификатору.
        /// </summary>
        /// <param name="roomId">Идентификатор комнаты.</param>
        /// <param name="activeOnly">Если true, возвращаются только активные удобства.</param>
        /// <param name="ct">Токен отмены для асинхронной операции.</param>
        /// <returns>Список удобств.</returns>
        Task<IEnumerable<Anemities>> GetAmenitiesByRoomIdAsync(Guid roomId, bool activeOnly = true, CancellationToken ct = default);
        /// <summary>
        /// Изменяет состояние активности удобства(как тупо это звучит) по его идентификатору. Если удобство активно, оно станет неактивным, и наоборот.
        /// </summary>
        /// <param name="id">Идентификатор удобства.</param>
        /// <param name="ct">Токен отмены для асинхронной операции.</param>
        /// <returns>True, если состояние было успешно изменено; иначе false.</returns>
        Task<bool> ChangeStateAsync(Guid id, CancellationToken ct = default);


    }
}

