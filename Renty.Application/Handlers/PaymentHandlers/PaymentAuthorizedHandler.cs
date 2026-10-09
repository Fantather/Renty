using MediatR;
using Renty.Application.Commands.PaymentCommands;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.LookupsTables;
using Renty.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.PaymentHandlers
{
    public class PaymentAuthorizedHandler : IRequestHandler<PaymentAuthorizedCommand, Unit>
    {
        private readonly IPaymentService _paymentService;
        private readonly IBookingRepository _bookingRepository;
        public PaymentAuthorizedHandler(
            IPaymentService paymentService,
            IBookingRepository bookingRepository)
        {
            _paymentService = paymentService;
            _bookingRepository = bookingRepository;
        }
        public async Task<Unit> Handle(PaymentAuthorizedCommand request, CancellationToken cancellationToken)
        {
            if (Guid.TryParse(request.BookingId, out var bookingId))
            {
                var booking = await _bookingRepository.GetByIdAsync(bookingId, cancellationToken);
                if (booking == null)
                    return Unit.Value;

                booking.PaymentStatus = PaymentStatusEnum.Authorized;
                booking.UpdatedAt = DateTime.UtcNow;

                await _bookingRepository.UpdateAsync(booking, cancellationToken);
            }

            return Unit.Value;
            
        }
    }
}
