using Renty.Web.Models.InputModels.Properties;
using Renty.Web.Models.Shared;

namespace Renty.Web.Models.PropertyCreate
{
    public class AmenitiesPageViewModel
    {
        public PropertyInputModel Property { get; set; } = new();
        public List<AmenityViewModel> Amenities { get; set; } = new();
    }
}
