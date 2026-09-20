using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.CreateProperty;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Queries.Property
{
    /// <summary>
    /// Запрос на получение недвижимости в статусе черновика
    /// </summary>
    /// <param name="PropertyId">Идентификатор недвижимости</param>
    /// <param name="CurrentUserId">Текущий пользователь</param>
    public record GetPropertyDraftQuery(Guid? PropertyId, Guid CurrentUserId) : IRequest<OperationResult<PropertyDraftDto>>;
}
