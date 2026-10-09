using MediatR;
using Renty.Application.Commands.PaymentCommands;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.LookupsTables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.PaymentHandlers
{
    public class PaymentFailedHandler : IRequestHandler<PaymentFailedCommand, Unit>
    {
        private readonly IBookingRepository _bookingRepository;
        public PaymentFailedHandler(
            IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }
        public async Task<Unit> Handle(PaymentFailedCommand request, CancellationToken cancellationToken)
        {
            if (Guid.TryParse(request.BookingId, out var bookingId))
            {
                var booking = await _bookingRepository.GetByIdAsync(bookingId, cancellationToken);
                if (booking == null)
                    return Unit.Value;

                booking.PaymentStatus = PaymentStatusEnum.Failed;
                // Если без повторных попыток
                // booking.Status = BookingStatusEnum.Cancelled;
                booking.UpdatedAt = DateTime.UtcNow;

                await _bookingRepository.UpdateAsync(booking, cancellationToken);
            }
            else
            {
                return Unit.Value;
            }

            return Unit.Value;
        }
    }
}
