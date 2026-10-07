using Renty.Domain.Models.User;

namespace Renty.Domain.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        /// <summary>
        /// Получить все отзывы для конкретного объекта недвижимости по его идентификатору
        /// </summary>
        /// <param name="propertyId">Идентификатор объекта недвижимости</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Список отзывов</returns>
        Task<IEnumerable<Review>> GetReviewsByPropertyIdAsync(Guid propertyId, CancellationToken ct = default);

        /// <summary>
        /// пагинированные отзывы по айди
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<(IEnumerable<Review> Reviews, int TotalCount)> GetReviewsByPropertyIdPaginatedAsync(Guid propertyId, int page, int pageSize, CancellationToken ct = default);

        /// <summary>
        /// пагинированные отзывы по слагу
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<(IEnumerable<Review> Reviews, int TotalCount)> GetReviewsByPropertySlugPaginatedAsync(string slug, int page, int pageSize, CancellationToken ct = default);


        /// <summary>
        /// Получить все отзывы, оставленные конкретным пользователем по его идентификатору
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Список отзывов</returns>
        Task<IEnumerable<Review>> GetReviewsByUserIdAsync(Guid userId, CancellationToken ct = default);

        /// <summary>
        /// Метод для хоста, чтобы ответить на отзыв
        /// </summary>
        /// <param name="reviewId">Идентификатор отзыва</param>
        /// <param name="response">Ответ хоста</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>True, если ответ был успешно добавлен, иначе false</returns>
        Task<bool> AddHostResponseAsync(Guid reviewId, string response, CancellationToken ct = default);
    }
}
