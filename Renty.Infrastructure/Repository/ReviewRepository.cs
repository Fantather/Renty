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
                .AsNoTracking()
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await _dbSet
                .Where(r => r.UserId == userId)

                .Include(r => r.Property)
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
    }
}
