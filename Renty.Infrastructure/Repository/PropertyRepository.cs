using Microsoft.EntityFrameworkCore;
using Renty.Domain.Interfaces;
using Renty.Domain.Models;
using Renty.Domain.Models.LookupsTables;
using Renty.Domain.Models.Properties;
using Renty.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

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
        public async Task<Property?> GetPropertyWithDetailsAsync(string slug)
        {
            return await GetFullQueryWithIncludes()
                .FirstOrDefaultAsync(p => p.Slug == slug);
        }

        /// <summary>
        /// Получение полного объекта Property с его связанными сущностями по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор объекта Property.</param>
        /// <returns>
        /// Полный объект Property с его связанными сущностями, если найден; иначе null.
        /// </returns>
        public async Task<Property?> GetPropertyWithDetailsAsync(Guid id)
        {
            return await GetFullQueryWithIncludes()
                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        ///<summary>
        ///Принимает уникальный идентификатор категории, как или слаг так или айди
        ///фильтрует по городу и категории, возвращает список объектов Property с их связанными сущностями.
        ///</summary>
        ///<param name="cityId">Идентификатор города для фильтрации.</param>
        ///<param name="categoryId">Идентификатор категории для фильтрации.</param>
        ///<param name="categorySlug">Слаг категории для фильтрации.</param>
        ///<returns>
        ///Список объектов Property с их связанными сущностями, соответствующих указанным фильтрам. Или пустой список, если ничего не подошло
        ///</returns>
        public async Task<IEnumerable<Property>> GetPropertiesForCatalogAsync(
           Guid? cityId = null,
           Guid? categoryId = null,
           string? categorySlug = null)
        {
            var query = _dbSet
                .Where(p => p.Status == PropertyStatusEnum.Active)
                .Include(p => p.City)
                .Include(p => p.PropertyImages)
                .AsQueryable();

            if (cityId.HasValue)
            {
                query = query.Where(p => p.CityId == cityId.Value);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }
             //не элс тут осознанно, врятли что то не так пойдет
            if (!string.IsNullOrEmpty(categorySlug))
            {
                query = query
                    .Include(p => p.Category) 
                    .Where(p => p.Category.Slug == categorySlug);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Получает все объекты Property, принадлежащие указанному хосту (по ID или UserName), включая связанные изображения.
        /// </summary>
        /// <param name="hostId">Идентификатор хоста для фильтрации.</param>
        /// <param name="username">Имя пользователя для фильтрации.</param>
        /// <returns>Список объектов Property, принадлежащих указанному хосту, включая связанные изображения.</returns>
        public async Task<IEnumerable<Property>> GetPropertiesByHostAsync(Guid? hostId = null, string? username = null)
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
            return await query.AsNoTracking().ToListAsync();
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
