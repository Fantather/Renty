namespace Renty.Web.Models.Properties
{
    // Одна комната/спальное место
    public class RoomViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string RoomType { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
