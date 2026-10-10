using MediatR;
using Microsoft.EntityFrameworkCore;
using Renty.Application.Commands.BookingCommands;
using Renty.Application.Common;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.LookupsTables;
using Renty.Infrastructure.Data;

namespace Renty.Application.Handlers.BookingHandlers
{
    public class CompleteBookingPaymentHandler : IRequestHandler<CompleteBookingPaymentCommand, OperationResult<bool>>
    {
        private readonly AppDbContext _context;
        private readonly IPaymentService _paymentService;

        public CompleteBookingPaymentHandler(AppDbContext context, IPaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;
        }

        public async Task<OperationResult<bool>> Handle(CompleteBookingPaymentCommand request, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == request.BookingId && b.UserId == request.CurrentUserId, cancellationToken);

            if (booking == null || string.IsNullOrEmpty(booking.PaymentIntentId))
                return OperationResult<bool>.Fail("Бронирование не найдено");

            if (booking.Status == BookingStatusEnum.Confirmed)
                return OperationResult<bool>.Success(true);

            var intent = await _paymentService.GetPaymentIntentAsync(booking.PaymentIntentId, cancellationToken);

            if (intent.Status != "succeeded")
                return OperationResult<bool>.Fail("Оплата не прошла");

            booking.Status = BookingStatusEnum.Confirmed;
            booking.PaymentStatus = PaymentStatusEnum.Completed;
            booking.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);

            return OperationResult<bool>.Success(true);
        }
    }
}
