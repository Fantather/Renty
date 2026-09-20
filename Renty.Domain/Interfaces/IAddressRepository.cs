using Renty.Domain.Models.Locations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.Interfaces
{
    public interface IAddressRepository
    {
        /// <summary>
        /// Найти адрес по идентификатору места
        /// </summary>
        /// <param name="placeId"></param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Найденный адрес или null</returns>
        Task<Address?> GetByPlaceIdAsync(string placeId, CancellationToken ct = default);

        Task AddAsync(Address address, CancellationToken ct = default);

        Task<Address?> GetByLocationAsync(double latitude, double longitude, CancellationToken ct = default);
    }
}
