using MediatR;
using Renty.Application.Commands.PaymentCommands;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.LookupsTables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.PaymentHandlers
{
    public class PaymentCanceledHandler : IRequestHandler<PaymentCanceledCommand, Unit>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IPaymentService _paymentService;
        public PaymentCanceledHandler(
            IBookingRepository bookingRepository,
            IPaymentService paymentService)
        {
            _bookingRepository = bookingRepository;
            _paymentService = paymentService;

        }
        public async Task<Unit> Handle(PaymentCanceledCommand request, CancellationToken cancellationToken)
        {
            if (Guid.TryParse(request.BookingId, out var bookingId))
            { 
                var booking = await _bookingRepository.GetByIdAsync(bookingId, cancellationToken);
                if (booking == null)
                    return Unit.Value;

                booking.PaymentStatus = PaymentStatusEnum.Canceled;
                booking.Status = BookingStatusEnum.Cancelled;
                booking.UpdatedAt = DateTime.UtcNow;

                await _bookingRepository.UpdateAsync(booking, cancellationToken);
            }
            else
            {
                return Unit.Value;
            }

            //await _paymentService.CancelPaymentAsync(request.PaymentIntentId, cancellationToken);

            return Unit.Value;
        }
    }
}
