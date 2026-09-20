namespace Renty.Web.Models.InputModels.Properties
{
    /// <summary>
    /// Основная модель для создания нового объявления
    /// </summary>
    /// <remarks>
    /// Важно возвоащать айди квартиры что бы подгрузить потом фотографии
    /// </remarks>
    // Эти поля пока нигде не используются: страницы под них ещё не сделаны, поэтому класс оставлен как есть.
    public class PropertyInputModel
    {
        public Guid PropertyId { get; set; }

        public TimeSpan? CheckInTime { get; set; }
        public TimeSpan? CheckOutTime { get; set; }

        public string? HouseRules { get; set; }

        public int? Floor { get; set; }
        public int? FloorsCount { get; set; }

        // комнаты создаются отдельно, тут необязательны
        public List<CreateRoomInputModel> Rooms { get; set; } = new();
    }
}
