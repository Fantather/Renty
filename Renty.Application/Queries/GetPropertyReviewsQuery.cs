using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetReviews;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Queries
{
    /// <summary>
    /// Запрос на отзывы под недвижимостью
    /// </summary>
    /// <param name="PropertyId">Идентификатор недвижимости</param>
    ///// <param name="Page">Текущая страница отзывов</param>
    ///// <param name="PageSize">Количество отзывов на страницу</param>
    public record GetPropertyReviewsQuery(Guid PropertyId/*, int Page, int PageSize*/) : IRequest<OperationResult<GetReviewsResponse>>;

}
