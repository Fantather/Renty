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
    public class RefundBookingHandler : IRequestHandler<RefundBookingCommand, OperationResult<Unit>>
    {

        private readonly OwnedBookingService _ownedBookingService;
        private readonly IPaymentService _paymentService;

        public RefundBookingHandler(OwnedBookingService ownedBookingService, IPaymentService paymentService)
        {
            _ownedBookingService = ownedBookingService;
            _paymentService = paymentService;
        }

        public async Task<OperationResult<Unit>> Handle(RefundBookingCommand request, CancellationToken ct)
        {
            // Возврат инициирует хозяин.
            var result = await _ownedBookingService.GetBookingForHostAsync(request.BookingId, request.CurrentUserId, ct);
            if (!result.IsSuccess)
                return OperationResult<Unit>.Fail(result.Errors.ToArray());

            var booking = result.Data!;

            if (booking.PaymentMethod != PaymentMethodType.Card)
                return OperationResult<Unit>.Fail("Возврат доступен только для оплаты картой");

            if (booking.PaymentStatus != PaymentStatusEnum.Completed)
                return OperationResult<Unit>.Fail("Возврат возможен только после списания средств");

            await _paymentService.RefundPaymentAsync(booking.PaymentIntentId!, ct);

            // Статус Refunded проставит вебхук charge.refunded
            return OperationResult<Unit>.Success(Unit.Value);
        }
    }
}
