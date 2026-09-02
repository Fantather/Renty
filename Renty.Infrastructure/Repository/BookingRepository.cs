using Microsoft.EntityFrameworkCore;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.LookupsTables;
using Renty.Domain.Models.Orders;
using Renty.Domain.Parameters;
using Renty.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Renty.Infrastructure.Repository
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> ChangeBookingStatusAsync(Guid bookingId, BookingStatusEnum newStatus, CancellationToken ct = default)
        {
            var booking = await _dbSet.FindAsync(new object[] { bookingId }, ct);

            if (booking == null || booking.Status == newStatus)
            {
                return false;
            }

            booking.Status = newStatus;
            booking.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> ChangePaymentStatusAsync(Guid bookingId, PaymentStatusEnum newStatus, CancellationToken ct = default)
        {
            var booking = await _dbSet.FindAsync(new object[] { bookingId }, ct);

            if (booking == null || booking.PaymentStatus == newStatus)
            {
                return false;
            }

            booking.PaymentStatus = newStatus;
            booking.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<IEnumerable<Booking>> GetActiveBookingsForCalendarAsync(Guid propertyId, CancellationToken ct = default)
        {
            var today = DateTime.UtcNow.Date;

            return await _dbSet
                .AsNoTracking()
                .Where(b => b.PropertyId == propertyId &&
                            b.CheckOutDate >= today &&
                            b.Status != BookingStatusEnum.Cancelled)
                .OrderBy(b => b.CheckInDate)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Booking>> GetPropertyBookingsAsync(Guid propertyId, Guid ownerId, ParametersBookings? param = null, CancellationToken ct = default)
        {
            var query = _dbSet
                    .Where(b => b.PropertyId == propertyId && b.Property.HostId == ownerId)
                    .Include(b => b.User)
                    .AsQueryable();

            var result = PrivatePagination(query, param);
            return await result.AsNoTracking().ToListAsync(ct);
        }

        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(Guid userId, ParametersBookings? param = null, CancellationToken ct = default)
        {
            var query = _dbSet
            .Where(b => b.UserId == userId)
            .Include(b => b.Property)
            .ThenInclude(p => p.City)
            .AsQueryable();

            var result = PrivatePagination(query, param);

            return await result.AsNoTracking().ToListAsync(ct);
        }

        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(string username, ParametersBookings? param = null, CancellationToken ct = default)
        {
            if(string.IsNullOrWhiteSpace(username))
    {
                return Enumerable.Empty<Booking>();
            }

            var query = _dbSet
                .Where(b => b.User.UserName == username)
                .Include(b => b.Property)
                    .ThenInclude(p => p.City)
                .AsQueryable();

            var result = PrivatePagination(query, param);

            return await result.AsNoTracking().ToListAsync(ct);
        }

        private IQueryable<Booking> PrivatePagination(IQueryable<Booking> query, ParametersBookings? param = null)
        {
            param ??= new ParametersBookings();

            // фильтры
            if (param.Status.HasValue)
            {
                query = query.Where(b => b.Status == param.Status.Value);
            }

            if (param.PaymentStatus.HasValue)
            {
                query = query.Where(b => b.PaymentStatus == param.PaymentStatus.Value);
            }

            if (param.PropertyId.HasValue)
            {
                query = query.Where(b => b.PropertyId == param.PropertyId.Value);
            }

            if (param.FromDate.HasValue)
            {
                query = query.Where(b => b.CheckInDate >= param.FromDate.Value);
            }

            if (param.ToDate.HasValue)
            {
                query = query.Where(b => b.CheckInDate <= param.ToDate.Value);
            }
            //cортировка
            query = param.SortBy switch
            {
                "CHECK_IN_ASC" => query.OrderBy(b => b.CheckInDate),
                "CHECK_IN_DESC" => query.OrderByDescending(b => b.CheckInDate),
                "CREATED_AT_ASC" => query.OrderBy(b => b.CreatedAt),
                "CREATED_AT_DESC" => query.OrderByDescending(b => b.CreatedAt),
                "PRICE_ASC" => query.OrderBy(b => b.TotalPrice),
                "PRICE_DESC" => query.OrderByDescending(b => b.TotalPrice),
                _ => query.OrderByDescending(b => b.CreatedAt)
            };

            return query.Skip(param.Skip).Take(param.PageSize);
        }

        public async Task<bool> IsDateRangeAvailableAsync(Guid propertyId, DateTime checkIn, DateTime checkOut, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}