using Renty.Web.Models.Shared;

namespace Renty.Web.Models.PropertyDrafts
{
    public class PropertyDraftsViewModel
    {
        public List<PropertyCardViewModel> Drafts { get; set; } = new();
        public List<PropertyCardViewModel> Published { get; set; } = new();
    }
}
