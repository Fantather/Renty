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
        Task<T?> GetByIdAsync(Guid id);

        //возвращение всех сущностей
        Task<IEnumerable<T>> GetAllAsync();
        //круд
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        //есть ли это уже в бд?
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}