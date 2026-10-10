using MediatR;
using Microsoft.EntityFrameworkCore;
using Renty.Application.Common;
using Renty.Application.Queries.Booking;
using Renty.Domain.Enums;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.LookupsTables;
using Renty.Infrastructure.Data;

namespace Renty.Application.Handlers.BookingHandlers
{
    public class GetBookingPaymentHandler : IRequestHandler<GetBookingPaymentQuery, OperationResult<string>>
    {
        private readonly AppDbContext _context;
        private readonly IPaymentService _paymentService;

        public GetBookingPaymentHandler(AppDbContext context, IPaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;
        }

        public async Task<OperationResult<string>> Handle(GetBookingPaymentQuery request, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == request.BookingId && b.UserId == request.CurrentUserId, cancellationToken);

            if (booking == null || booking.PaymentMethod != PaymentMethodType.Card || string.IsNullOrEmpty(booking.PaymentIntentId))
                return OperationResult<string>.Fail("Бронирование не найдено");

            if (booking.Status != BookingStatusEnum.Pending)
                return OperationResult<string>.Fail("Бронирование уже оплачено");

            var intent = await _paymentService.GetPaymentIntentAsync(booking.PaymentIntentId, cancellationToken);

            return OperationResult<string>.Success(intent.ClientSecret);
        }
    }
}
