using MediatR;
using Renty.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.PropertyCommands
{
    public record SavePropertyPricingCommand(Guid PropertyId, Guid CurrentUserId, decimal PricePerNight, string Currency, int? WeekendPricePercent = 0) : IRequest<OperationResult<Guid>>;
}
