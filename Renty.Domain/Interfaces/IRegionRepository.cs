using Renty.Domain.Models.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.Interfaces
{
    public interface IRegionRepository: IGenericRepository<Region>
    {
        /// <summary>
        /// Получить регионы по идентификатору страны
        /// </summary>
        /// <param name="countryId">Идентификатор страны</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Список регионов</returns>
        Task<IEnumerable<Region>> GetRegionsByCountryIdAsync(Guid countryId, CancellationToken ct = default);
        
    }
}
