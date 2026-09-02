using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
namespace Renty.Domain.Interfaces
{
    
    public interface IGenericRepository<T> where T : class
        /// <summary>
        /// попытка интерфейса репозитория для всех сущностей что бы не повторяться
        /// </summary>
    {
        /// <summary>
        /// Получает сущность по её идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сущности</param>
        /// <returns>Сущность или null, если не найдена</returns>
        Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
        /// <summary>
        /// Получает все сущности
        /// </summary>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Список всех сущностей</returns>
        Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
        /// <summary>
        /// Добавляет новую сущность
        /// </summary>
        /// <param name="entity">Сущность для добавления</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        Task AddAsync(T entity, CancellationToken ct = default);
        /// <summary>
        /// Обновляет существующую сущность
        /// </summary>
        /// <param name="entity">Сущность для обновления</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        Task UpdateAsync(T entity, CancellationToken ct = default);
        /// <summary>
        /// Удаляет сущность
        /// </summary>
        /// <param name="entity">Сущность для удаления</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        Task DeleteAsync(T entity, CancellationToken ct = default);

        /// <summary>
        /// Проверяет, существует ли сущность, соответствующая заданному условию
        /// </summary>
        /// <param name="predicate">Условие для проверки</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>True, если сущность существует; иначе false</returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    }
}