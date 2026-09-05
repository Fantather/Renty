using Renty.Web.Models.Shared;

namespace Renty.Web.Models.Home
{
    public class HomeIndexViewModel
    {
        public List<PropertyCardViewModel> Properties { get; set; } = new();
        public CategoryStripViewModel CategoryStrip { get; set; } = new();
        public PropertyFilterViewModel Filter { get; set; } = new();
    }
}
