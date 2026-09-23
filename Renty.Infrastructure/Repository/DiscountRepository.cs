using Microsoft.EntityFrameworkCore;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.Properties;
using Renty.Infrastructure.Data;


namespace Renty.Infrastructure.Repository
{
    public class DiscountRepository : GenericRepository<Discount>, IDiscountRepository
    {
        public DiscountRepository(AppDbContext context) : base(context)
        {
        }

        public async Task AddRangeAsync(IEnumerable<Discount> discounts, CancellationToken ct = default)
        {
            await _dbSet.AddRangeAsync(discounts, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<Discount>> GetActiveByPropertyIdAsync(Guid propertyId, bool noTracking = true, CancellationToken ct = default)
        {
            var query = _dbSet.Where(d => d.PropertyId == propertyId && d.IsActive);

            if (noTracking)
                query = query.AsNoTracking();

            return await query.ToListAsync(ct);
        }
        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}
