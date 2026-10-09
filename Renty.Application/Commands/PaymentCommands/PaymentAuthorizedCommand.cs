using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.PaymentCommands
{
    public record PaymentAuthorizedCommand(string PaymentIntentId, string BookingId) : IRequest<Unit>;
}
