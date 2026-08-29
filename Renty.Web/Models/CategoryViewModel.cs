namespace Renty.Web.Models
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string IconName { get; set; } = string.Empty;
    }
}
