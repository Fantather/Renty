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

        public async Task<IEnumerable<Discount>> GetActiveByPropertyIdAsync(Guid propertyId, CancellationToken ct = default)
        {
            return await _dbSet
                .Where(d => d.PropertyId == propertyId && d.IsActive)
                .AsNoTracking()
                .ToListAsync(ct);
        }
    }
}