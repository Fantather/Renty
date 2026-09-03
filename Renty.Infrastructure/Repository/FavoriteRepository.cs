using Microsoft.EntityFrameworkCore;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.User;
using Renty.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Infrastructure.Repository
{
    public class FavoriteRepository : GenericRepository<Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Favorite>> GetUserFavoritesAsync(Guid userId, CancellationToken ct = default)
        {
            return await _dbSet
                .Where(f => f.UserId == userId)
                .Include(f => f.Property)
                    .ThenInclude(p => p.City) 
                .Include(f => f.Property)
                    .ThenInclude(p => p.PropertyImages) 
                .AsNoTracking()
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<bool> IsFavoriteAsync(Guid userId, Guid propertyId, CancellationToken ct = default)
        {
            return await _dbSet
                .AnyAsync(f => f.UserId == userId && f.PropertyId == propertyId, ct);
        }

        public async Task<bool> ToggleFavoriteAsync(Guid userId, Guid propertyId, CancellationToken ct = default)
        {
            var favorite = await _dbSet
                .FirstOrDefaultAsync(f => f.UserId == userId && f.PropertyId == propertyId, ct);

            if (favorite != null)
            {
                // Если уже есть в избранном - удаляем
                _dbSet.Remove(favorite);
            }
            else
            {
                // Если нет - добавляем
                favorite = new Favorite
                {
                    UserId = userId,
                    PropertyId = propertyId,
                    CreatedAt = DateTime.UtcNow
                };
                await _dbSet.AddAsync(favorite, ct);
            }

            await _context.SaveChangesAsync(ct);
            return favorite == null; 
        }
    }
}
