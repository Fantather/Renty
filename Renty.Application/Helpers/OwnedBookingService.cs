using Renty.Application.Common;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Helpers
{
    public class OwnedBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        public OwnedBookingService(
            IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<OperationResult<Booking>> GetBookingForHostAsync(Guid bookingId, Guid currentUserId, CancellationToken ct = default)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId, ct);

            if (booking == null)
                return OperationResult<Booking>.Fail("The booking does not exist");

            if (booking.Property.HostId != currentUserId)
                return OperationResult<Booking>.Fail("Access denied");

            return OperationResult<Booking>.Success(booking);
        }
    }
}
