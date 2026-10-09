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
    public class ConfirmBookingHandler : IRequestHandler<ConfirmBookingCommand, OperationResult<Unit>>
    {
        private readonly OwnedBookingService _ownedBookingService;
        private readonly IBookingRepository _bookingRepository;
        private readonly IPaymentService _paymentService;
        public ConfirmBookingHandler(
            OwnedBookingService ownedBookingService,
            IBookingRepository bookingRepository,
            IPaymentService paymentService)
        {
            _ownedBookingService = ownedBookingService;
            _bookingRepository = bookingRepository;
            _paymentService = paymentService;
        }
        public async Task<OperationResult<Unit>> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
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

                await _paymentService.CapturePaymentAsync(booking.PaymentIntentId!, cancellationToken);
            }

            booking.Status = BookingStatusEnum.Confirmed;
            await _bookingRepository.UpdateAsync(booking, cancellationToken);

            return OperationResult<Unit>.Success(Unit.Value);
        }
    }
}
