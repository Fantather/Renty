
using Microsoft.EntityFrameworkCore;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.Locations;
using Renty.Domain.Models.LookupsTables;
using Renty.Infrastructure.Data;
using Renty.Domain.Models.Properties;

namespace Renty.Infrastructure.Repository
{

    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        public CityRepository(AppDbContext context) : base(context)
        {
        }
        /// <summary>
        ///  Возвращает список городов по идентификатору страны.
        /// </summary>
        /// <param name="countryId">Идентификатор страны</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Список городов</returns>
        public async Task<IEnumerable<City>> GetCitiesByCountryAsync(Guid countryId, CancellationToken ct = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(c => c.CountryId == countryId)
                .OrderBy(c => c.Name)
                .ToListAsync(ct);
        }
        /// <summary>
        ///  Возвращает список городов по названию страны.
        /// </summary>
        /// <param name="countryName">Название страны</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Список городов</returns>
        public async Task<IEnumerable<City>> GetCitiesByCountryAsync(string countryName, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(countryName))
            {
                return Enumerable.Empty<City>();
            }

            var nameLower = countryName.ToLower();

            return await _dbSet
                .AsNoTracking()
                .Where(c => c.Country.Name.ToLower() == nameLower)
                .OrderBy(c => c.Name)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Возвращает список городов, в которых есть квартиры из базы данных. Он делает выборку только активных квартир и возвращает уникальные города, отсортированные по имени.
        /// </summary>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Список городов</returns>
        public async Task<IEnumerable<City>> GetCitiesWithApartmentsAsync(CancellationToken ct = default)
        {
            return await _context.Set<Property>()
                .Where(p => p.Status == PropertyStatusEnum.Active)
                .Select(p => p.City)
                .Distinct()
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync(ct);
        }
        //Он будет принимать название города в виде строки и возвращать мне список городов в которых есть квартиры из БД
        /// <summary>
        /// Auto-complete поиск для Алексея. Принимает название города в виде строки и возвращает список городов, в которых есть квартиры из базы данных.
        /// </summary>
        /// <param name="searchTerm"> название города для поиска</param>
        /// <param name="limit"> максимальное количество результатов</param>
        /// <param name="ct"> токен отмены</param>
        /// <returns> список городов, соответствующих критериям поиска</returns>
        public async Task<IEnumerable<City>> SearchCitiesByNameAsync(string searchTerm, int limit = 10, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Enumerable.Empty<City>();
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
                return Enumerable.Empty<City>();//по одной букве будет жесть искать
            }
            else if (searchTerm.Length > 100)
            {
                searchTerm = searchTerm.Substring(0, 100);
            }

            var term = searchTerm + "%";


            return await _dbSet
                .AsNoTracking() //для легкости ибо тянет в автокомлит
                .Where(c => EF.Functions.ILike(c.Name, term))
                .Where(c => _context.Set<Property>().Any(p => p.CityId == c.Id && p.Status == PropertyStatusEnum.Active))
                .OrderBy(c => c.Name)
                .Take(limit)
                .ToListAsync(ct);
        }

        public async Task<bool> ChangeState(Guid cityId, bool isActive, CancellationToken ct = default)
        {
            var city = _dbSet.Find(cityId);
            if (city == null)
            {
                return false;
            }
            if (city.IsActive == isActive)
            {
                return false; //Ничего не сменилось камон
            }
            city.IsActive = isActive;
            await _context.SaveChangesAsync(ct);
            return true;
        }


        public async Task<bool> ChangeState(string name, bool isActive, CancellationToken ct = default)
        {
            var city = _dbSet.FirstOrDefault(c => c.Name == name);
            if (city == null)
            {
                return false;
            }
            if (city.IsActive == isActive)
            {
                return false; //Ничего не сменилось камон
            }
            city.IsActive = isActive;
            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}

