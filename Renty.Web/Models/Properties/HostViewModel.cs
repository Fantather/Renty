namespace Renty.Web.Models.Properties
{
    public class HostViewModel
    {
        public string? AvatarUrl { get; set; }
        public string FullName { get; set; } = string.Empty;
        public bool IsSuperhost { get; set; }
        public string ResponseSpeed { get; set; } = string.Empty;
        public int YearsHosting { get; set; }
    }
}
