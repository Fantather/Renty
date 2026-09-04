using Renty.Domain.Models.Locations;


namespace Renty.Domain.Interfaces
{
    public interface ICityRepository : IGenericRepository<City>
    {
        //Он будет принимать название города в виде строки и возвращать мне список городов в которых есть квартиры из БД
        /// <summary>
        /// Auto-complete поиск для Алексея. Принимает название города в виде строки и возвращает список городов, в которых есть квартиры из базы данных.
        /// </summary>
        /// <param name="searchTerm"> название города для поиска</param>
        /// <param name="limit"> максимальное количество результатов</param>
        /// <param name="ct"> токен отмены</param>
        /// <returns> список городов, соответствующих критериям поиска</returns>
        Task<IEnumerable<City>> SearchCitiesByNameAsync(string searchTerm, int limit = 10, CancellationToken ct = default);
        /// <summary>
        /// Возвращает список городов, в которых есть квартиры из базы данных. Он делает выборку только активных квартир и возвращает уникальные города, отсортированные по имени.
        /// </summary>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Список городов</returns>
        Task<IEnumerable<City>> GetCitiesWithApartmentsAsync(CancellationToken ct = default);
        /// <summary>
        ///  Возвращает список городов по идентификатору страны.
        /// </summary>
        /// <param name="countryId">Идентификатор страны</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Список городов</returns>
        Task<IEnumerable<City>> GetCitiesByCountryAsync(Guid countryId, CancellationToken ct = default);
        /// <summary>
        ///  Возвращает список городов по названию страны.
        /// </summary>
        /// <param name="countryName">Название страны</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Список городов</returns>
        Task<IEnumerable<City>> GetCitiesByCountryAsync(string countryName, CancellationToken ct = default);

        /// <summary>
        /// Изменяет состояние объекта City по его идентификатору.
        /// </summary>
        /// <param name="cityId">Идентификатор города.</param>
        /// <param name="isActive">Новое состояние активности.</param>
        /// <param name="ct">Токен отмены.</param>
        /// <returns>Возвращает true, если состояние было успешно изменено, иначе false.</returns>
        Task<bool> ChangeState(Guid cityId, bool isActive, CancellationToken ct = default);
        /// <summary>
        /// Изменяет состояние объекта City по его слагу.
        /// </summary>
        /// <param name="name">название города.</param>
        /// <param name="isActive">Новое состояние активности.</param>
        /// <param name="ct">Токен отмены.</param>
        /// <returns>Возвращает true, если состояние было успешно изменено, иначе false.</returns>
        Task<bool> ChangeState(string name, bool isActive, CancellationToken ct = default);

    }
}
