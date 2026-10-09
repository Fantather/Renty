using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.PaymentCommands
{
    public record PaymentRefundedCommand(string PaymentIntentId, bool Refunded) : IRequest<Unit>;
}
