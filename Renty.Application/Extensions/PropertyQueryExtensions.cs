using Renty.Domain.Models.LookupsTables;
using Renty.Domain.Models.Properties;

namespace Renty.Application.Extensions
{
    /// <summary>
    /// общая логика фильтрации для свойств, включая количество гостей, даты заезда и выезда, категорию и удобства
    /// </summary>
    /// <param name="query"></param>
    /// <param name="guestCount"></param>
    /// <param name="checkInDate"></param>
    /// <param name="checkOutDate"></param>
    /// <param name="categorySlug"></param>
    /// <param name="amenityIds"></param>
    /// <returns></returns>
    public static class PropertyQueryExtensions
    {
        public static IQueryable<Property> ApplyFilters(
            this IQueryable<Property> query,
            int? guestCount,
            DateTime? checkInDate,
            DateTime? checkOutDate,
            string? categorySlug,
            List<Guid>? amenityIds,
            bool? petsAllowed)
        {
            if (guestCount.HasValue && guestCount > 0)
            {
                query = query.Where(p => p.Details.MaxGuests >= guestCount);
            }

            if (!string.IsNullOrEmpty(categorySlug))
            {
                query = query.Where(p => p.Category.Slug == categorySlug);  
            }

            if (petsAllowed.HasValue && petsAllowed.Value == true)
            {
                query = query.Where(p => p.Details.PetsAllowed);
            }

            if (amenityIds != null && amenityIds.Any())
            {
                foreach (var amenityId in amenityIds)
                {
                    query = query.Where(p => p.PropertyAmenities.Any(pa => pa.AmenityId == amenityId && pa.IsActive));
                }
            }

            if (checkInDate.HasValue && checkOutDate.HasValue)
            {
                var ci = DateTime.SpecifyKind(checkInDate.Value, DateTimeKind.Utc);
                var co = DateTime.SpecifyKind(checkOutDate.Value, DateTimeKind.Utc);

                query = query.Where(p => !p.Bookings.Any(b =>
                    (b.Status == BookingStatusEnum.Confirmed || b.Status == BookingStatusEnum.Pending) &&
                    b.CheckOutDate > ci &&
                    b.CheckInDate < co));
            }
            else if (checkInDate.HasValue)
            {
                var ci = DateTime.SpecifyKind(checkInDate.Value, DateTimeKind.Utc);
                var ciEnd = ci.AddDays(1);

                query = query.Where(p => !p.Bookings.Any(b =>
                    (b.Status == BookingStatusEnum.Confirmed || b.Status == BookingStatusEnum.Pending) &&
                    b.CheckOutDate > ci &&
                    b.CheckInDate < ciEnd));
            }
            else if (checkOutDate.HasValue)
            {
                var co = DateTime.SpecifyKind(checkOutDate.Value, DateTimeKind.Utc);
                var coStart = DateTime.SpecifyKind(co.Date, DateTimeKind.Utc);
                var coEnd = coStart.AddDays(1);

                query = query.Where(p => !p.Bookings.Any(b =>
                    (b.Status == BookingStatusEnum.Confirmed || b.Status == BookingStatusEnum.Pending) &&
                    b.CheckOutDate > coStart &&
                    b.CheckInDate < coEnd));
            }

            return query;
        }

        // Общая логика генерации строки длительности пребывания
        public static string GetDurationString(DateTime? checkInDate, DateTime? checkOutDate)
        {
            if (checkInDate.HasValue && checkOutDate.HasValue)
            {
                var checkIn = checkInDate.Value;
                var checkOut = checkOutDate.Value;
                return $"{checkIn:dd MMM} - {checkOut:dd MMM} ({(checkOut - checkIn).Days} ночей)";
            }
            return string.Empty;
        }
    }
}
