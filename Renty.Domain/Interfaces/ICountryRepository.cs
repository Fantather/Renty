using System;
using System.Collections.Generic;
using System.Text;
using Renty.Domain.Models.Locations;

namespace Renty.Domain.Interfaces
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        /// <summary>
        /// Получить список стран по подстроке, где есть наличие городов, которые имеют хотя бы один активный объект недвижимости.
        /// </summary>
        /// <param name="searchTerm">Подстрока для поиска стран.</param>
        /// <param name="limit">Максимальное количество стран для возврата.</param>
        /// <param name="ct">Токен отмены.</param>
        /// <returns>Список стран, соответствующих критериям поиска.</returns>
        Task<IEnumerable<Country>> GetCountriesByNameAsync(string searchTerm, int limit = 10, CancellationToken ct = default);
        /// <summary>
        /// Изменяет состояние объекта Country по его идентификатору.
        /// </summary>
        /// <param name="countryId">Идентификатор страны.</param>
        /// <param name="isActive">Новое состояние активности.</param>
        /// <param name="ct">Токен отмены.</param>
        /// <returns>Возвращает true, если состояние было успешно изменено, иначе false.</returns>
        Task<bool> ChangeState(Guid countryId, bool isActive, CancellationToken ct = default);
        /// <summary>
        /// Изменяет состояние объекта Country по его слагу.
        /// </summary>
        /// <param name="name">название страны.</param>
        /// <param name="isActive">Новое состояние активности.</param>
        /// <param name="ct">Токен отмены.</param>
        /// <returns>Возвращает true, если состояние было успешно изменено, иначе false.</returns>
        Task<bool> ChangeState(string name, bool isActive, CancellationToken ct = default);
    }

}
