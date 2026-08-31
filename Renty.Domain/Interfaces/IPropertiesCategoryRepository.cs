using Renty.Domain.Parameters;
using Renty.Domain.Models;

namespace Renty.Domain.Interfaces
{
    public interface IPropertiesCategoryRepository : IGenericRepository<PropertiesCategory>
    {

        // получение категории с полями и связями по Slug 
        Task<PropertiesCategory?> GetCategoryWithDetailsAsync(string slug, CancellationToken ct = default);
        Task<PropertiesCategory?> GetCategoryByNameAsync(string name, CancellationToken ct = default);

        // Выборка категорий
        Task<IEnumerable<PropertiesCategory>> GetAllActiveAsync(CancellationToken ct = default);

        // Проверка активности категории по id и slug
        Task<bool> IsCategotyActiveAsync(Guid id, CancellationToken ct = default);   
        Task<bool> IsCategotyActiveAsync(string slug, CancellationToken ct = default);

        //активно - неактивно(мягкое удаление и возвращение)
        Task<bool> ChangeStateCategoryAsync(Guid id, bool state, CancellationToken ct = default);
        Task<bool> ChangeStateCategoryAsync(string slug, bool state, CancellationToken ct = default);


        //нужно?
        //Task<IEnumerable<PropertiesCategory>> GetCategoriesAsync(CategoryParameters parameters, CancellationToken ct = default);

    }
}
