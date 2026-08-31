using Microsoft.EntityFrameworkCore;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.LookupsTables;
using Renty.Domain.Models.Properties;
using Renty.Domain.Parameters;
using Renty.Infrastructure.Data;



namespace Renty.Infrastructure.Repository
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {

        public PropertyRepository(AppDbContext context) : base(context)
        {
        }
        ///<summary>
        ///Получение полного объекта Property с его связанными сущностями по идентификатору.
        /// </summary>
        /// <param name="slug">Slug объекта Property.</param>
        /// <returns>
        /// Полный объект Property с его связанными сущностями, если найден; иначе null.
        /// </returns>
        public async Task<Property?> GetPropertyWithDetailsAsync(string slug, CancellationToken ct = default)
        {
            return await GetFullQueryWithIncludes()
                
                .FirstOrDefaultAsync(p => p.Slug == slug, ct);
        }

        /// <summary>
        /// Получение полного объекта Property с его связанными сущностями по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор объекта Property.</param>
        /// <returns>
        /// Полный объект Property с его связанными сущностями, если найден; иначе null.
        /// </returns>
        public async Task<Property?> GetPropertyWithDetailsAsync(Guid id, CancellationToken ct = default)
        {
            return await GetFullQueryWithIncludes()
                 .FirstOrDefaultAsync(p => p.Id == id,ct);
        }

        ///<summary>
        ///Принимает уникальный идентификатор категории, как или слаг так или айди
        ///фильтрует по городу и категории, возвращает список объектов Property с их связанными сущностями.
        ///</summary>
        ///<param name="skip">Количество пропущенных объектов</param>
        ///<param name="pageSize">Количество объектов на странице</param>
        ///<param name="cityId">Идентификатор города для фильтрации.</param>
        ///<param name="categoryId">Идентификатор категории для фильтрации.</param>
        ///<param name="categorySlug">Слаг категории для фильтрации.</param>
        ///<param name="sortBy">Сортировка по параметру</param>
        ///<param name="checkInDate">Фильтрация по дате заселения</param>
        ///<param name="checkOutDate">Фильтрация по дате выезда</param>
        ///<param name="guestCount">Фильтрация по колличеству гостей</param>
        ///<param name="amenityIds">Список идентификаторов удобств для фильтрации.</param>
        ///<param name="ct">Токен отмены для асинхронной операции.</param>
        ///<returns>
        ///Список объектов Property с их связанными сущностями, соответствующих указанным фильтрам. Или пустой список, если ничего не подошло
        ///</returns>

        public async Task<IEnumerable<Property>> GetPropertiesForCatalogAsync(ParametersPropertiesForCatalog param, CancellationToken ct = default)
        {
            var query = _dbSet
                .Where(p => p.Status == PropertyStatusEnum.Active)
                .Include(p => p.City)
                .Include(p => p.PropertyImages)
                .AsQueryable();

            if (param.GuestCount.HasValue)
            {
                query = query.Where(p => p.Details.MaxGuests >= param.GuestCount);
            }

            if (param.CheckInDate.HasValue && param.CheckOutDate.HasValue)
            {
                query = query.Where(p => !p.Bookings.Any(b => b.CheckOutDate > param.CheckInDate.Value && b.CheckInDate < param.CheckOutDate.Value));
            }

            if (param.CityId.HasValue)
            {
                query = query.Where(p => p.CityId == param.CityId.Value);
            }

            if (param.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == param.CategoryId.Value);
            }
             //не элс тут осознанно, врятли что то не так пойдет
            if (!string.IsNullOrEmpty(param.CategorySlug))
            {
                query = query
                    .Include(p => p.Category) 
                    .Where(p => p.Category.Slug == param.CategorySlug);
            }

            if (param.AmenityIds != null && param.AmenityIds.Any())
            {
                foreach (var amenityId in param.AmenityIds)
                {
                    // в объекте должно быть КАЖДОЕ удобство из списка
                    query = query.Where(p => p.PropertyAmenities
                        .Any(pa => pa.AmenityId == amenityId && pa.IsActive));
                }
            }

            query = param.SortBy switch
            {
                "RATING_ASC" => query.OrderBy(p => p.AverageRating),
                "RATING_DESC" => query.OrderByDescending(p => p.AverageRating),
                "PRICEPRENIGHT_ASC" => query.OrderBy(p => p.PricePerNight),
                "PRICEPRENIGHT_DESC" => query.OrderByDescending(p => p.PricePerNight),
                "CREATED_AT_ASC" => query.OrderBy(p => p.CreatedAt),
                "CREATED_AT_DESC" => query.OrderByDescending(p => p.CreatedAt),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };
            
            

            return await query
                .Skip(param.Skip)
                .Take(param.PageSize)
                .AsNoTracking().ToListAsync(ct);
        }

        /// <summary>
        /// Получает все объекты Property, принадлежащие указанному хосту (по ID или UserName), включая связанные изображения.
        /// </summary>
        /// <param name="hostId">Идентификатор хоста для фильтрации.</param>
        /// <param name="username">Имя пользователя для фильтрации.</param>
        /// <returns>Список объектов Property, принадлежащих указанному хосту, включая связанные изображения.</returns>
        public async Task<IEnumerable<Property>> GetPropertiesByHostAsync(Guid? hostId = null, string? username = null, CancellationToken ct = default)
        {
            var query = _dbSet
                .Include(p => p.PropertyImages)
                .AsQueryable();

            if (hostId.HasValue)
            {
                query = query.Where(p => p.HostId == hostId.Value);
            }

            else if (!string.IsNullOrEmpty(username))
            {
                query = query.Where(p => p.Host.UserName == username);
            }
            else 
            {
                return Enumerable.Empty<Property>(); 
            }
            return await query.AsNoTracking().ToListAsync(ct);
        }

        ////////////////////////////////////////////////////////////////////////////////////////
        /*
         * Приватный метод для получения полного запроса с включением связанных сущностей Property.
         * Возвращает: Полный запрос IQueryable<Property> с включением связанных сущностей
         */
        private IQueryable<Property> GetFullQueryWithIncludes()
        {
            return _dbSet
                .Include(p => p.Details)
                .Include(p => p.Host)
                .Include(p => p.City)
                .Include(p => p.Country)
                .Include(p => p.Category)
                .Include(p => p.PropertyImages)
                .Include(p => p.PropertyAmenities)
                .Include(p => p.PropertyTags)
                    .ThenInclude(pt => pt.Tag);
        }
    }
}
