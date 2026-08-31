using Microsoft.EntityFrameworkCore;
using Renty.Domain.Interfaces;
using Renty.Domain.Models;
using Renty.Infrastructure.Data;

namespace Renty.Infrastructure.Repository
{
    public class PropertiesCategoryRepository : GenericRepository<PropertiesCategory>, IPropertiesCategoryRepository
    {
        public PropertiesCategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> ChangeStateCategoryAsync(Guid id, bool state, CancellationToken ct)
        {
            var category = await _dbSet.FirstOrDefaultAsync(c => c.Id == id, ct);
            if (category == null)
            {
                return false;
            }

            category.IsActive = state;
            category.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> ChangeStateCategoryAsync(string slug, bool state, CancellationToken ct)
        {
            var category = await _dbSet.FirstOrDefaultAsync(c => c.Slug == slug, ct);
            if (category == null)
            {
                return false;
            }

            category.IsActive = state;
            category.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<IEnumerable<PropertiesCategory>> GetAllActiveAsync(CancellationToken ct)
        {
            return await _dbSet.Where(c => c.IsActive)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<PropertiesCategory?> GetCategoryByNameAsync(string name, CancellationToken ct)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.Name == name, ct);
        }

        public async Task<PropertiesCategory?> GetCategoryAsync(Guid id, CancellationToken ct)
        {
           return await _dbSet
                .FirstOrDefaultAsync(c => c.Id == id, ct);
        }

        public async Task<PropertiesCategory?> GetCategoryWithDetailsAsync(string slug, CancellationToken ct)
        {
            return await _dbSet
                .Include(c => c.Properties)
                .FirstOrDefaultAsync(c => c.Slug == slug, ct);
        }

        public async Task<bool> IsCategotyActiveAsync(Guid id, CancellationToken ct)
        {
            return await _dbSet.AnyAsync(c => c.Id == id && c.IsActive, ct);
        }

        public async Task<bool> IsCategotyActiveAsync(string slug, CancellationToken ct)
        {
            return await _dbSet.AnyAsync(c => c.Slug == slug && c.IsActive, ct);
        }
    }
}
