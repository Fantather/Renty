using Renty.Web.Models.InputModels.Properties;
using Renty.Web.Models.PropertyCreate;

namespace Renty.Web.Models.PropertyEdit
{
    public class LocationEditPageViewModel
    {
        public string AddressSummary { get; set; } = string.Empty;
        public bool ShowExactLocation { get; set; }
        public LocationInputModel Location { get; set; } = new();
        public AddressInputModel Address { get; set; } = new();
        public LocationVisibilityPageViewModel Visibility { get; set; } = new();
    }
}
