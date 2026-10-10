using MediatR;
using Microsoft.EntityFrameworkCore;
using Renty.Application.Common;
using Renty.Application.DTOs.Booking;
using Renty.Application.Queries.Booking;
using Renty.Domain.Models.LookupsTables;
using Renty.Infrastructure.Data;

namespace Renty.Application.Handlers.BookingHandlers
{
    public class GetUserBookingsHandler : IRequestHandler<GetUserBookingsQuery, OperationResult<List<UserBookingDto>>>
    {
        private readonly AppDbContext _context;

        public GetUserBookingsHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<List<UserBookingDto>>> Handle(GetUserBookingsQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _context.Bookings
                .AsNoTracking()
                .Where(b => b.UserId == request.UserId && b.Status != BookingStatusEnum.Cancelled)
                .OrderByDescending(b => b.CheckInDate)
                .Select(b => new UserBookingDto
                {
                    BookingId = b.Id,
                    PropertyId = b.PropertyId,
                    PropertySlug = b.Property.Slug,
                    ImageUrls = b.Property.PropertyImages
                        .OrderByDescending(i => i.IsPrimary)
                        .ThenBy(i => i.DisplayOrder)
                        .Select(i => i.ImageUrl)
                        .ToList(),
                    City = string.IsNullOrEmpty(b.Property.City.NameRu) ? b.Property.City.Name : b.Property.City.NameRu,
                    Country = string.IsNullOrEmpty(b.Property.Country.NameRu) ? b.Property.Country.Name : b.Property.Country.NameRu,
                    CategoryName = b.Property.Category != null ? b.Property.Category.Name : string.Empty,
                    Rating = b.Property.AverageRating,
                    PricePerNight = b.Property.PricePerNight,
                    TotalPrice = b.TotalPrice,
                    CheckInDate = b.CheckInDate,
                    CheckOutDate = b.CheckOutDate
                })
                .ToListAsync(cancellationToken);

            return OperationResult<List<UserBookingDto>>.Success(bookings);
        }
    }
}
