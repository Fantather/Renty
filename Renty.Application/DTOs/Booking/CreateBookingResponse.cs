using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.Booking
{
    public class CreateBookingResponse
    {
        public Guid BookingId { get; set; } 
        public string? ClientSecret { get; set; } = null!;
    }
}
