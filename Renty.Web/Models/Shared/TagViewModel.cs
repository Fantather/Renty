namespace Renty.Web.Models.Shared
{
    // Один тег недвижимости (Тихое, Уникальное и т.д.)
    public class TagViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IconName { get; set; } = string.Empty;
    }
}
