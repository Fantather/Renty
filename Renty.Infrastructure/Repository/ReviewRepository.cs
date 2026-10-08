using Microsoft.EntityFrameworkCore;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.User;
using Renty.Infrastructure.Data;


namespace Renty.Infrastructure.Repository
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Review>> GetReviewsByPropertyIdAsync(Guid propertyId, CancellationToken ct = default)
        {
            return await _dbSet
                .Where(r => r.PropertyId == propertyId)
                .Include(r => r.User)            
                .Include(r => r.Property)
                    .ThenInclude(p => p.Host)    
                .AsNoTracking()
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await _dbSet
                .Where(r => r.UserId == userId)

                .Include(r => r.Property).ThenInclude(p => p.Host)
                .AsNoTracking()
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<bool> AddHostResponseAsync(Guid reviewId, string response, CancellationToken ct = default)
        {
            var review = await _dbSet.FindAsync(new object[] { reviewId }, ct);
            if (review == null)
            {
                return false;
            }

            review.HostResponse = response;
            review.HostResponseDate = DateTime.UtcNow;
            review.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);
            return true;
        }
        ///
        public async Task<(IEnumerable<Review> Reviews, int TotalCount)> GetReviewsByPropertyIdPaginatedAsync(Guid propertyId, int page, int pageSize, CancellationToken ct = default)
        {
            var query = _dbSet.Where(r => r.PropertyId == propertyId);

            var totalCount = await query.CountAsync(ct);

            var reviews = await query
                .Include(r => r.User)
                .Include(r => r.Property)
                    .ThenInclude(p => p.Host)
                .AsNoTracking()
                .OrderByDescending(r => r.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (reviews, totalCount);
        }

        public async Task<(IEnumerable<Review> Reviews, int TotalCount)> GetReviewsByPropertySlugPaginatedAsync(string slug, int page, int pageSize, CancellationToken ct = default)
        {
            var query = _dbSet.Where(r => r.Property.Slug == slug);

            var totalCount = await query.CountAsync(ct);

            var reviews = await query
                .Include(r => r.User)
                .Include(r => r.Property)
                    .ThenInclude(p => p.Host)
                .AsNoTracking()
                .OrderByDescending(r => r.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return (reviews, totalCount);
        }
    }
}
