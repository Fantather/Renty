namespace Renty.Web.Models.Shared
{
    public class CounterRowViewModel
    {
        public string Key { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Subtitle { get; set; }
        public int Min { get; set; } = 0;
    }
}
