using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.PaymentCommands
{
    public record PaymentCapturedCommand(string PaymentIntentId, string BookingId) : IRequest<Unit>;
}
