using Renty.Web.Models.InputModels.Properties;

namespace Renty.Web.Models.PropertyCreate
{
    public class LocationVisibilityPageViewModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public LocationVisibilityInputModel Input { get; set; } = new();
    }
}
