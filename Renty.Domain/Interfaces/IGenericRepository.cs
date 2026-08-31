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
        //возвращение одной сущности 
        Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);

        //возвращение всех сущностей
        Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
        //круд
        Task AddAsync(T entity, CancellationToken ct = default);
        Task UpdateAsync(T entity, CancellationToken ct = default);
        Task DeleteAsync(T entity, CancellationToken ct = default);

        //есть ли это уже в бд?
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    }
}