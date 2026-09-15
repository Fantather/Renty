using Renty.Domain.Models.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.Interfaces
{
    public interface IPropertyImageRepository
    {
        /// <summary>
        /// Удаление изображения для недвижимости
        /// </summary>
        /// <param name="image">Изображение для удаления</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns></returns>
        Task DeleteAsync(PropertyImage image, CancellationToken ct = default);
        /// <summary>
        /// Добавление списка изображений для недвижимости
        /// </summary>
        /// <param name="images">Список изображений</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns></returns>
        Task AddRangeAsync(IEnumerable<PropertyImage> images, CancellationToken ct = default);
        /// <summary>
        /// Обновление данных списка изображений
        /// </summary>
        /// <param name="images">Список изображений</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns></returns>
        Task UpdateAsync(IEnumerable<PropertyImage> images, CancellationToken ct = default);
        /// <summary>
        /// Сохранение изменений
        /// </summary>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns></returns>
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
