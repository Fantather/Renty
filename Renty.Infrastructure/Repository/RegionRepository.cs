using Microsoft.EntityFrameworkCore;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.Locations;
using Renty.Domain.Models.LookupsTables;
using Renty.Domain.Models.Properties;
using Renty.Domain.Parameters;
using Renty.Infrastructure.Data;



namespace Renty.Infrastructure.Repository
{
    public class RegionRepository : GenericRepository<Region>, IRegionRepository
    {

        public RegionRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Region>> GetRegionsByCountryIdAsync(Guid countryId, CancellationToken ct = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(c => c.CountryId == countryId)
                .OrderBy(c => c.Name)
                .ToListAsync(ct);
        }
    }
}
