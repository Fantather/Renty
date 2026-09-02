using System;
using System.Collections.Generic;
using System.Text;
using Renty.Domain.Models.Properties;
using Renty.Domain.Models.Properties.Anemities;
using Renty.Domain.Parameters;
using Renty.Domain.Models.LookupsTables;

namespace Renty.Domain.Interfaces
{
   public interface IPropertyRepository : IGenericRepository<Property>
    {
        /// <summary>
        /// Изменяет состояние объекта Property по его идентификатору.
        /// </summary>
        /// <param name="propertyId">Идентификатор объекта Property.</param>
        /// <param name="newState">Новое состояние объекта Property.</param>
        /// <returns>True, если состояние было успешно изменено; иначе false.</returns>
        public bool ChangeState(Guid propertyId, PropertyStatusEnum newState);
        /// <summary>
        /// Изменяет состояние объекта Property по его слагу.
        /// </summary>
        /// <param name="slug">Слаг объекта Property.</param>
        /// <param name="newState">Новое состояние объекта Property.</param>
        /// <returns>True, если состояние было успешно изменено; иначе false.</returns>
        public bool ChangeState(string slug, PropertyStatusEnum newState);

        /// <summary>
        /// Получение полного объекта Property с его связанными сущностями по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор объекта Property.</param>
        /// <returns>
        /// Полный объект Property с его связанными сущностями, если найден; иначе null.
        /// </returns>
        Task<Property?> GetPropertyWithDetailsAsync(Guid id, CancellationToken ct = default);
        ///<summary>
        ///Получение полного объекта Property с его связанными сущностями по идентификатору.
        /// </summary>
        /// <param name="slug">Slug объекта Property.</param>
        /// <returns>
        /// Полный объект Property с его связанными сущностями, если найден; иначе null.
        /// </returns>
        Task<Property?> GetPropertyWithDetailsAsync(string slug, CancellationToken ct = default);

        ///<summary>
        ///Принимает уникальный идентификатор категории, как или слаг так или айди
        ///фильтрует по городу и категории, возвращает список объектов Property с их связанными сущностями.
        ///</summary>
        ///<param name="skip">Количество пропущенных объектов</param>
        ///<param name="pageSize">Количество объектов на странице</param>
        ///<param name="cityId">Идентификатор города для фильтрации.</param>
        ///<param name="categoryId">Идентификатор категории для фильтрации.</param>
        ///<param name="categorySlug">Слаг категории для фильтрации.</param>
        ///<param name="sortBy">Сортировка по параметру</param>
        ///<param name="checkInDate">Фильтрация по дате заселения</param>
        ///<param name="checkOutDate">Фильтрация по дате выезда</param>
        ///<param name="guestCount">Фильтрация по колличеству гостей</param>
        ///<param name="amenityIds">Список идентификаторов удобств для фильтрации.</param>
        ///<param name="ct">Токен отмены для асинхронной операции.</param>
        ///<returns>
        ///Список объектов Property с их связанными сущностями, соответствующих указанным фильтрам. Или пустой список, если ничего не подошло
        ///</returns>
        Task<IEnumerable<Property>> GetPropertiesForCatalogAsync(ParametersPropertiesForCatalog param, CancellationToken ct = default
        );

        /// <summary>
        /// Получает все объекты Property, принадлежащие указанному хосту (по ID или UserName), включая связанные изображения.
        /// </summary>
        /// <param name="hostId">Идентификатор хоста для фильтрации.</param>
        /// <param name="username">Имя пользователя для фильтрации.</param>
        /// <returns>Список объектов Property, принадлежащих указанному хосту, включая связанные изображения.</returns>
        Task<IEnumerable<Property>> GetPropertiesByHostAsync(Guid? hostId = null, string? username = null, CancellationToken ct = default);

    }
}
