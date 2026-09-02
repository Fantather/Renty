using Renty.Domain.Parameters;
using Renty.Domain.Models;

namespace Renty.Domain.Interfaces
{
    public interface IPropertiesCategoryRepository : IGenericRepository<PropertiesCategory>
    {

        /// <summary>
        /// Возвращает категорию свойств по слагу.
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<PropertiesCategory?> GetCategoryWithDetailsAsync(string slug, CancellationToken ct = default);
        
        /// <summary>
        /// Возвращает категорию свойств по имени.
        /// </summary>
        /// <param name="name">Имя категории</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Категория свойств или null, если не найдена</returns>
        Task<PropertiesCategory?> GetCategoryByNameAsync(string name, CancellationToken ct = default);

        /// <summary>
        /// Возвращает все активные категории свойств.
        /// </summary>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>Список активных категорий свойств</returns>
        Task<IEnumerable<PropertiesCategory>> GetAllActiveAsync(CancellationToken ct = default);

        /// <summary>
        /// Проверяет, активна ли категория по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор категории</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>True, если категория активна; иначе false</returns>
        Task<bool> IsCategotyActiveAsync(Guid id, CancellationToken ct = default);   
        /// <summary>
        /// Проверяет, активна ли категория по её слагу.
        /// </summary>
        /// <param name="slug">Слаг категории</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>True, если категория активна; иначе false</returns>
        Task<bool> IsCategotyActiveAsync(string slug, CancellationToken ct = default);

        /// <summary>
        /// Изменяет состояние активности категории по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор категории</param>
        /// <param name="state">Новое состояние активности</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>True, если состояние было успешно изменено; иначе false</returns>
        Task<bool> ChangeStateCategoryAsync(Guid id, bool state, CancellationToken ct = default);
        /// <summary>
        /// Изменяет состояние активности категории по её слагу.
        /// </summary>
        /// <param name="slug">Слаг категории</param>
        /// <param name="state">Новое состояние активности</param>
        /// <param name="ct">Токен отмены для асинхронной операции</param>
        /// <returns>True, если состояние было успешно изменено; иначе false</returns>
        Task<bool> ChangeStateCategoryAsync(string slug, bool state, CancellationToken ct = default);


        //нужно?
        //Task<IEnumerable<PropertiesCategory>> GetCategoriesAsync(CategoryParameters parameters, CancellationToken ct = default);

    }
}
