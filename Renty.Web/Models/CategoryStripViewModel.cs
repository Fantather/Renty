namespace Renty.Web.Models
{
    public class CategoryStripViewModel
    {
        public List<CategoryViewModel> Categories { get; set; } = new();
        public string? SelectedSlug { get; set; }
    }
}
