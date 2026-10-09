using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.PaymentCommands
{
    public record PaymentCanceledCommand(string PaymentIntentId, string BookingId) : IRequest<Unit>;
}
