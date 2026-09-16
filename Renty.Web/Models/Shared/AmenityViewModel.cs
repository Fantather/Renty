namespace Renty.Web.Models.Shared
{
    // Одно удобство (Wi-Fi, кухня и т.д.)
    public class AmenityViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string IconName { get; set; } = string.Empty;
    }
}
