using Renty.Web.Models.InputModels.Properties;
using Renty.Web.Models.Shared;

namespace Renty.Web.Models.PropertyCreate
{
    public class TagsPageViewModel
    {
        public TagsInputModel Input { get; set; } = new();
        public List<TagViewModel> Tags { get; set; } = new();
    }
}
