using Renty.Domain.Interfaces;
using Renty.Domain.Models.Media;
using Renty.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Infrastructure.Repository
{
    public class PropertyImageRepository : GenericRepository<PropertyImage>, IPropertyImageRepository
    {
        public PropertyImageRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Удаление изображения для недвижимости
        /// </summary>
        /// <param name="image">Изображение для удаления</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns></returns>
        public async Task DeleteAsync(PropertyImage image, CancellationToken ct = default)
        {
            _dbSet.Remove(image);
            await _context.SaveChangesAsync(ct);
        }
        /// <summary>
        /// Добавление списка изображений для недвижимости
        /// </summary>
        /// <param name="images">Список изображений</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns></returns>
        public async Task AddRangeAsync(IEnumerable<PropertyImage> images, CancellationToken ct = default)
        {
            await _dbSet.AddRangeAsync(images, ct);
            await _context.SaveChangesAsync(ct);
        }
        /// <summary>
        /// Обновление данных списка изображений
        /// </summary>
        /// <param name="images">Список изображений</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns></returns>
        public async Task UpdateAsync(IEnumerable<PropertyImage> images, CancellationToken ct = default)
        {
            _dbSet.UpdateRange(images);
            await _context.SaveChangesAsync(ct);
        }

        public async Task SaveChangesAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync();
        }
    }
}
