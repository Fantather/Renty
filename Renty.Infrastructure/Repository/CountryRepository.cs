using Microsoft.EntityFrameworkCore;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.Locations;
using Renty.Domain.Models.LookupsTables;
using Renty.Domain.Models.Properties;
using Renty.Infrastructure.Data;

namespace Renty.Infrastructure.Repository
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        public CountryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Country>> GetCountriesByNameAsync(string searchTerm, int limit = 10, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Enumerable.Empty<Country>();
            }
            if (limit <= 0)
            {
                limit = 10;
            }
            else if (limit > 100)
            {
                limit = 100;
            }

            if (searchTerm.Length < 2)
            {
                return Enumerable.Empty<Country>();//по одной букве будет жесть искать
            }
            else if (searchTerm.Length > 100)
            {
                searchTerm = searchTerm.Substring(0, 100);
            }

            var term = searchTerm + "%";


            return await _dbSet
                .AsNoTracking() // для легкости, ибо тянет в автокомплит
                .Where(c => EF.Functions.ILike(c.Name, term))
                .Where(c => _context.Set<Property>().Any(p => p.CountryId == c.Id && p.Status == PropertyStatusEnum.Active))
                .OrderBy(c => c.Name)
                .Take(limit) 
                .ToListAsync(ct);
        }

        public async Task<bool> ChangeState(Guid countryId, bool isActive, CancellationToken ct = default)
        {
            var country = _dbSet.Find(countryId);
            if (country == null)
            {
                return false;
            }
            if (country.IsActive == isActive)
            {
                return false; //Ничего не сменилось камон
            }
            country.IsActive = isActive;
            await _context.SaveChangesAsync(ct);
            return true;
        }

   
        public async Task<bool> ChangeState(string name, bool isActive, CancellationToken ct = default)
        {
            var country = _dbSet.FirstOrDefault(c => c.Name == name);
            if (country == null)
            {
                return false;
            }
            if (country.IsActive == isActive)
            {
                return false; //Ничего не сменилось камон
            }
            country.IsActive = isActive;
            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
    }


