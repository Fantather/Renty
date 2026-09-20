using Renty.Web.Models.InputModels.Properties;
using Renty.Web.Models.Shared;

namespace Renty.Web.Models.PropertyCreate
{
    public class CategoryPageViewModel
    {
        public CategoryInputModel Input { get; set; } = new();
        public List<CategoryViewModel> Categories { get; set; } = new();
    }
}
