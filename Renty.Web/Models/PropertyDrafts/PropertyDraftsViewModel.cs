using Renty.Web.Models.Shared;

namespace Renty.Web.Models.PropertyDrafts
{
    public class PropertyDraftsViewModel
    {
        public List<UserPropertyCardViewModel> Drafts { get; set; } = new();
        public List<UserPropertyCardViewModel> Published { get; set; } = new();
    }
}
