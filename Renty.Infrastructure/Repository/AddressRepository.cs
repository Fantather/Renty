using Microsoft.EntityFrameworkCore;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.Locations;
using Renty.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Infrastructure.Repository
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Найти адрес по идентификатору места
        /// </summary>
        /// <param name="placeId"></param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Найденный адрес или null</returns>
        public async Task<Address?> GetByPlaceIdAsync(string placeId, CancellationToken ct = default)
        {
            return await _dbSet
                .Include(a => a.City)
                .FirstOrDefaultAsync(a => a.PlaceId == placeId);
        }

        public async Task AddAsync(Address address, CancellationToken ct = default)
        {
            await _dbSet.AddAsync(address, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<Address?> GetByLocationAsync(double latitude ,double longitude, CancellationToken ct = default)
        {
            return await _dbSet
                .Include(a => a.City)
                .FirstOrDefaultAsync(a => a.Location.Y == latitude && a.Location.X == longitude, ct);
        }
    }
}
