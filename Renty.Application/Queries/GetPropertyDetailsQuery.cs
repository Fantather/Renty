using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetProperty;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Queries
{
    /// <summary>
    /// Запрос на расширенную информацию недвижимости
    /// </summary>
    /// <param name="PropertySlug">Слаг для поиска недвижимости</param>
    /// <param name="UserId">Идентификатор текущего пользователя</param>
    public record GetPropertyDetailsQuery(string PropertySlug, Guid? UserId) : IRequest<OperationResult<GetPropertyDetailsResponse>>;
}
