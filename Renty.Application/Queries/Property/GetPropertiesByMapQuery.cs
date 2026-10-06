using Renty.Application.Common;
using Renty.Application.DTOs.GetProperties;
using MediatR;
using System;
using System.Collections.Generic;

namespace Renty.Application.Queries.Property
{
    /// <summary>
    /// Получение списка жилья для аренды по границам карты
    /// </summary>
    /// <param name="North">Северная граница карты</param>
    /// <param name="South">Южная граница карты</param>
    /// <param name="East">Восточная граница карты</param>
    /// <param name="West">Западная граница карты</param>
    /// <param name="Page">Текущая страница</param>
    /// <param name="PageSize">Количество объектов на странице</param>
    /// <param name="UserId">Текущий пользователь</param>
    /// <param name="CategorySlug">Идентификатор категории</param>
    /// <param name="SortBy">Сортировка по параметру</param>
    /// <param name="CheckInDate">Фильтрация по дате заселения</param>
    /// <param name="CheckOutDate">Фильтрация по дате выезда</param>
    /// <param name="GuestCount">Фильтрация по количеству гостей</param>
    /// <param name="Destination">Фильтрация по названию города или месту</param>
    /// <param name="PetsAllowed">Питомцы  можно ли</param>
    /// <param name="AmenityIds">Фильтрация по списку удобств (Wi-Fi, кухня и т.д.)</param>
    public record GetPropertiesByMapQuery(
        double North,
        double South,
        double East,
        double West,
        string? CategorySlug = null,  
        DateTime? CheckInDate = null,
        DateTime? CheckOutDate = null,
        int? GuestCount = null,
        string? Destination = null,
        Guid? UserId = null,
        int Page = 1,
        int PageSize = 20,
        List<Guid>? AmenityIds = null,
        string? SortBy = null,
        bool? PetsAllowed = null
    ) : IRequest<OperationResult<GetPropertiesByMapResponse>>;
}
