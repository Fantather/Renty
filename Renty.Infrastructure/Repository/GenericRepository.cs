using Microsoft.EntityFrameworkCore;
using Renty.Domain.Interfaces;
using Renty.Infrastructure.Data;
using System.Linq.Expressions;
namespace Renty.Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Общий репозиторий для работы с сущностями в базе данных
        /// </summary>
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        // зависимость
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        /// <summary>
        /// Получает сущность по её идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сущности</param>
        /// <returns>Сущность или null, если не найдена</returns>
        public async Task<T?> GetByIdAsync(Guid id)
        {

            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Получает все сущности
        /// </summary>
        /// <returns>Список всех сущностей</returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Добавляет новую сущность в базу данных
        /// </summary>  
        /// <param name="entity">Сущность для добавления</param>
        /// <returns>Задача, представляющая асинхронную операцию</returns>
        public async Task AddAsync(T entity)
        {


            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Обновляет существующую сущность в базе данных
        /// </summary>
        /// <param name="entity">Сущность для обновления</param>
        /// <returns>Задача, представляющая асинхронную операцию</returns>
        public async Task UpdateAsync(T entity)
        {

            _dbSet.Update(entity);

            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Удаляет существующую сущность из базы данных
        /// </summary>
        /// <param name="entity">Сущность для удаления</param>
        /// <returns>Задача, представляющая асинхронную операцию</returns>  
        public async Task DeleteAsync(T entity)
        {
    
            _dbSet.Remove(entity);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Проверяет, существует ли сущность, удовлетворяющая указанному условию
        /// </summary>
        /// <param name="predicate">Условие для проверки</param>
        /// <returns>Задача, представляющая асинхронную операцию, результатом которой является true, если сущность существует, иначе false</returns>
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }
    }
}