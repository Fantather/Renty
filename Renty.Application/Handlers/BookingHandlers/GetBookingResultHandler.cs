using MediatR;
using Microsoft.EntityFrameworkCore;
using Renty.Application.Common;
using Renty.Application.DTOs.Booking;
using Renty.Application.Queries.Booking;
using Renty.Infrastructure.Data;

namespace Renty.Application.Handlers.BookingHandlers
{
    public class GetBookingResultHandler : IRequestHandler<GetBookingResultQuery, OperationResult<BookingResultDto>>
    {
        private readonly AppDbContext _context;

        public GetBookingResultHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<BookingResultDto>> Handle(GetBookingResultQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.Bookings
                .AsNoTracking()
                .Where(b => b.Id == request.BookingId && b.UserId == request.CurrentUserId)
                .Select(b => new BookingResultDto
                {
                    BookingId = b.Id,
                    PropertyName = b.Property.Name ?? string.Empty,
                    PropertySlug = b.Property.Slug,
                    PropertyImageUrl = b.Property.PropertyImages
                        .OrderByDescending(i => i.IsPrimary)
                        .ThenBy(i => i.DisplayOrder)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault(),
                    City = string.IsNullOrEmpty(b.Property.City.NameRu) ? b.Property.City.Name : b.Property.City.NameRu,
                    Country = string.IsNullOrEmpty(b.Property.Country.NameRu) ? b.Property.Country.Name : b.Property.Country.NameRu,
                    CheckInDate = b.CheckInDate,
                    CheckOutDate = b.CheckOutDate,
                    GuestsCount = b.GuestsCount,
                    TotalPrice = b.TotalPrice,
                    Currency = b.Currency,
                    Status = b.Status
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (result == null)
                return OperationResult<BookingResultDto>.Fail("Бронирование не найдено");

            return OperationResult<BookingResultDto>.Success(result);
        }
    }
}
