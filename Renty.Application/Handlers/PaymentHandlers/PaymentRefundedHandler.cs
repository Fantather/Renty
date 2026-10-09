using MediatR;
using Renty.Application.Commands.PaymentCommands;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.LookupsTables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.PaymentHandlers
{
    public class PaymentRefundedHandler : IRequestHandler<PaymentRefundedCommand, Unit>
    {
        private readonly IBookingRepository _bookingRepository;
        public PaymentRefundedHandler(
            IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }
        public async Task<Unit> Handle(PaymentRefundedCommand request, CancellationToken cancellationToken)
        {
            if (!request.Refunded)
                return Unit.Value;

            var booking = await _bookingRepository.GetByPaymentIntentIdAsync(request.PaymentIntentId,cancellationToken);

            if (booking == null || booking.PaymentStatus == PaymentStatusEnum.Refunded)
                return Unit.Value; // идемпотентность

            booking.PaymentStatus = PaymentStatusEnum.Refunded;
            booking.Status = BookingStatusEnum.Cancelled;
            booking.UpdatedAt = DateTime.UtcNow;

            await _bookingRepository.UpdateAsync(booking, cancellationToken);
            return Unit.Value;
        }
    }
}
