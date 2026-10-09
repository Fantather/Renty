using MediatR;
using Renty.Application.Commands.BookingCommands;
using Renty.Application.Common;
using Renty.Application.Helpers;
using Renty.Domain.Enums;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.LookupsTables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.BookingHandlers
{
    public class DeclineBookingHandler : IRequestHandler<DeclineBookingCommand, OperationResult<Unit>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly OwnedBookingService _ownedBookingService;
        private readonly IPaymentService _paymentService;

        public DeclineBookingHandler(
            IBookingRepository bookingRepository,
            OwnedBookingService ownedBookingService,
            IPaymentService paymentService)
        {
            _bookingRepository = bookingRepository;
            _ownedBookingService = ownedBookingService;
            _paymentService = paymentService;
        }
        public async Task<OperationResult<Unit>> Handle(DeclineBookingCommand request, CancellationToken cancellationToken)
        {
            var result = await _ownedBookingService.GetBookingForHostAsync(request.BookingId, request.CurrentUserId, cancellationToken);

            if (!result.IsSuccess)
                return OperationResult<Unit>.Fail(result.Errors.ToArray());

            var booking = result.Data!;

            if (booking.Status != BookingStatusEnum.Pending)
                return OperationResult<Unit>.Fail("Бронирование уже обработано");

            if (booking.PaymentMethod == PaymentMethodType.Card)
            {
                if (booking.PaymentStatus != PaymentStatusEnum.Authorized)
                    return OperationResult<Unit>.Fail("Оплата ещё не подтверждена гостем");

                await _paymentService.CancelPaymentAsync(booking.PaymentIntentId!, cancellationToken);
            }

            booking.Status = BookingStatusEnum.Cancelled;
            await _bookingRepository.UpdateAsync(booking, cancellationToken);

            return OperationResult<Unit>.Success(Unit.Value);
        }
    }
}
