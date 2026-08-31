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
        /// <summary>
        /// Изменяет состояние активности категории по идентификатору или слагу.
        /// </summary>
        /// <param name="id">Идентификатор категории.</param>
        /// <param name="state">Активна ли на сайте эта категория.</param>
        /// <param name="ct">Токен отмены для асинхронной операции.</param>
        /// <returns>True, если состояние было успешно изменено; иначе false.</returns>
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
        /// <summary>
        /// Изменяет состояние активности категории по слагу.
        /// </summary>
        /// <param name="slug">Слаг категории.</param>
        /// <param name="state">Активна ли на сайте эта категория.</param>
        /// <param name="ct">Токен отмены для асинхронной операции.</param>
        /// <returns>True, если состояние было успешно изменено; иначе false.</returns>
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
        /// <summary>
        /// Возвращает все активные категории свойств.
        /// </summary>
        /// <param name="ct">Токен отмены для асинхронной операции.</param>
        /// <returns>Список всех активных категорий свойств.</returns>
        public async Task<IEnumerable<PropertiesCategory>> GetAllActiveAsync(CancellationToken ct)
        {
            return await _dbSet.Where(c => c.IsActive)
                .AsNoTracking()
                .ToListAsync(ct);
        }
        /// <summary>
        /// Возвращает категорию свойств по имени.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<PropertiesCategory?> GetCategoryByNameAsync(string name, CancellationToken ct)
        {
            return await _dbSet
                .FirstOrDefaultAsync(c => c.Name == name, ct);
        }
        /// <summary>
        /// Возвращает категорию свойств по идентификатору.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<PropertiesCategory?> GetCategoryAsync(Guid id, CancellationToken ct)
        {
           return await _dbSet
                .FirstOrDefaultAsync(c => c.Id == id, ct);
        }
        /// <summary>
        /// Возвращает категорию свойств по слагу.
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<PropertiesCategory?> GetCategoryWithDetailsAsync(string slug, CancellationToken ct)
        {
            return await _dbSet
                .Include(c => c.Properties)
                .FirstOrDefaultAsync(c => c.Slug == slug, ct);
        }
        /// <summary>
        /// Проверяет, активна ли категория свойств по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор категории свойств.</param>
        /// <param name="ct">Токен отмены для асинхронной операции.</param>
        /// <returns>True, если категория активна, иначе false.</returns>
        public async Task<bool> IsCategotyActiveAsync(Guid id, CancellationToken ct)
        {
            return await _dbSet.AnyAsync(c => c.Id == id && c.IsActive, ct);
        }

        /// <summary>
        /// Проверяет, активна ли категория свойств по слагу.
        /// </summary>
        /// <param name="slug">Слаг категории свойств.</param>
        /// <param name="ct">Токен отмены для асинхронной операции.</param>
        /// <returns>True, если категория активна, иначе false.</returns>
        public async Task<bool> IsCategotyActiveAsync(string slug, CancellationToken ct)
        {
            return await _dbSet.AnyAsync(c => c.Slug == slug && c.IsActive, ct);
        }
    }
}
