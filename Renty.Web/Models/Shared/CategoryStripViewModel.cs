namespace Renty.Web.Models.Shared
{
    public class CategoryStripViewModel
    {
        public List<CategoryViewModel> Categories { get; set; } = new();
        public string? SelectedSlug { get; set; }
    }
}
