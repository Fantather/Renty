using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetProperties.UserProperties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Queries.Property
{
    /// <summary>
    /// Запрос на все объявленя пользователя
    /// </summary>
    /// <param name="CurrentUserId">Текущий пользователь</param>
    public record GetUserPropertiesQuery(Guid CurrentUserId) : IRequest<OperationResult<GetUserPropertiesResponse>>;
}
