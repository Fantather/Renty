using Renty.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.Interfaces
{
    public interface IFavoriteRepository : IGenericRepository<Favorite>
    {
        /// <summary>
        /// Получить список избранных объектов у пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Список избранных объектов</returns>
        Task<IEnumerable<Favorite>> GetUserFavoritesAsync(Guid userId, CancellationToken ct = default);

        /// <summary>
        /// Проверяет, добавлена ли конкретная недвижимость в избранное у пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="propertyId">Идентификатор недвижимости</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>True, если недвижимость добавлена в избранное, иначе false</returns>
        Task<bool> IsFavoriteAsync(Guid userId, Guid propertyId, CancellationToken ct = default);

        /// <summary>
        /// Переключает состояние избранного для конкретной недвижимости у пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="propertyId">Идентификатор недвижимости</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>True, если недвижимость была добавлена в избранное, иначе false</returns>
        Task<bool> ToggleFavoriteAsync(Guid userId, Guid propertyId, CancellationToken ct = default);
    }
}
