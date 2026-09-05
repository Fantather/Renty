using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetCities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Queries
{
    /// <summary>
    /// Запрос городов для автокомплита
    /// </summary>
    /// <param name="SearchTerm">Текст поиска для автокомплита</param>
    /// <param name="Limit">Лимит найденых городов</param>
    public record GetCitiesQuery(string SearchTerm, int? Limit) : IRequest<OperationResult<GetCitiesResponse>>;
}
