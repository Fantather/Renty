using Microsoft.EntityFrameworkCore;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.Properties;
using Renty.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Infrastructure.Repository
{
    public class PropertyTagRepository : GenericRepository<Tag>, IPropertyTagRepository
    {
        public PropertyTagRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Guid>> GetExistingIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
        {
            return await _dbSet
                .Where(t => t.IsActive && ids.Contains(t.Id))
                .Select(t => t.Id).ToListAsync(ct);
        }

        public async Task<IEnumerable<Tag>> GetTagsByPropertyIdAsync(Guid propertyId, bool activeOnly = true, CancellationToken ct = default)
        {
            var query = _dbSet
                .Where(t => t.PropertyTags.Any(pt => pt.PropertyId == propertyId));
            if (activeOnly)
            {
                query.Where(t => t.IsActive);
            }
            return await query.ToListAsync(ct);
        }
    }
}
