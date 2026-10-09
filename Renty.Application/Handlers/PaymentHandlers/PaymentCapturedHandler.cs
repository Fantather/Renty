using MediatR;
using Renty.Application.Commands.PaymentCommands;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.LookupsTables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.PaymentHandlers
{
    public class PaymentCapturedHandler : IRequestHandler<PaymentCapturedCommand, Unit>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IPaymentService _paymentService;
        public PaymentCapturedHandler(
            IBookingRepository bookingRepository,
            IPaymentService paymentService)
        {
            _bookingRepository = bookingRepository;
            _paymentService = paymentService;
        }
        public async Task<Unit> Handle(PaymentCapturedCommand request, CancellationToken cancellationToken)
        {
            if (Guid.TryParse(request.BookingId, out var bookingId))
            {
                var booking = await _bookingRepository.GetByIdAsync(bookingId, cancellationToken);
                if (booking == null)
                    return Unit.Value;

                // Если Stripe прислал событие повторно
                if (booking.PaymentStatus == PaymentStatusEnum.Completed)
                    return Unit.Value;

                booking.PaymentStatus = PaymentStatusEnum.Completed;
                booking.UpdatedAt = DateTime.UtcNow;

                // Если это было автобронирование - подтверждаем сразу
                // Если это списание после ручного подтверждения хозяином - статус уже Confirmed
                if (booking.Status == BookingStatusEnum.Pending)
                    booking.Status = BookingStatusEnum.Confirmed;


                await _bookingRepository.UpdateAsync(booking, cancellationToken);
            }
            else
            {
                return Unit.Value;
            }

            //await _paymentService.CapturePaymentAsync(request.PaymentIntentId, cancellationToken);

            return Unit.Value;
        }
    }
}
