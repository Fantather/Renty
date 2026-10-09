using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.Booking;
using Renty.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.BookingCommands
{
    public record CreateBookingCommand(Guid CurrentUserId, Guid PropertyId, DateTime CheckInDate, DateTime CheckOutDate, int GuestsCount, PaymentMethodType PaymentMethod):IRequest<OperationResult<CreateBookingResponse>>;
}
