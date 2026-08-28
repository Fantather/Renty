namespace Renty.Domain.Models.Properties
{
    /// <summary>
    /// Типы комнат (Studio, One Bedroom, Suite, Deluxe, etc.)
    /// </summary>
    public class RoomType
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Активен ли тип комнаты
        public bool IsActive { get; set; } = true;

        //// Порядок отображения
        //public int DisplayOrder { get; set; } = 0;

        // Навигационные свойства
        public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
    }
}
