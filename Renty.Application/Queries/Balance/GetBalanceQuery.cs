using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.Balance;
using Renty.Domain.ServiceModels.Stripe;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Queries.Stripe
{
    public record GetBalanceQuery(Guid CurrentUserId) : IRequest<OperationResult<GetBalanceDto>>;
}
