using Renty.Web.Models.Shared;

namespace Renty.Web.Models.Trips
{
    public class TripsViewModel
    {
        public List<PropertyCardViewModel> Bookings { get; set; } = new();
        public List<PropertyCardViewModel> Favorites { get; set; } = new();
    }
}
