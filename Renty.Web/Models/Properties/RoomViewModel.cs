namespace Renty.Web.Models.Properties
{
    // Одна комната/спальное место
    public class RoomViewModel
    {
        // Заглушки вместо реальных RoomTypeId — RoomTypeDto их пока не присылает
        public static readonly Guid BedroomTypeId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        public static readonly Guid BathroomTypeId = Guid.Parse("00000000-0000-0000-0000-000000000002");

        public string Name { get; set; } = string.Empty;
        public Guid RoomTypeId { get; set; }
        public string? Description { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
