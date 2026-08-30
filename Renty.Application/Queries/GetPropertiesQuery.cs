using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetProperties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Queries
{
    /// <summary>
    /// Получение списка жилья для аренды и категориями
    /// </summary>
    /// <param name="Page">Текущая страница</param>
    /// <param name="PageSize">Количество объектов на странице</param>
    /// <param name="CityId">Идентификатор города для фильтрации</param>
    /// <param name="CategoryId">Идентификатор категории</param>
    /// <param name="CategorySlug">Идентификатор категории</param>
    /// <param name="SortBy">Сортировка по параметру</param>
    /// <param name="CheckInDate">Фильтрация по дате заселения</param>
    /// <param name="CheckOutDate">Фильтрация по дате выезда</param>
    /// <param name="GuestCount">Фильтрация по колличеству гостей</param>
    public record GetPropertiesQuery(int Page, int PageSize, Guid? CityId, Guid? CategoryId, string? CategorySlug,  string? SortBy, DateTime? CheckInDate, DateTime? CheckOutDate, int? GuestCount) : IRequest<OperationResult<GetPropertiesResponse>>;
}
