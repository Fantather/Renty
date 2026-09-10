using System.ComponentModel.DataAnnotations;

namespace Renty.Web.Models.InputModels.Properties
{
    /// <summary>
    /// Вложенная модель для создания комнаты внутри квартиры
    /// </summary>
    public class CreateRoomInputModel
    {
        [Required(ErrorMessage = "Выберите тип комнаты")]
        public Guid RoomTypeId { get; set; }

        [Required(ErrorMessage = "Укажите название (например, 'Главная спальня')")]
        public string Name { get; set; } = string.Empty;

        [Range(0, 10, ErrorMessage = "Некорректное количество спальных мест")]
        public int BedsCount { get; set; }

        [Range(1, 1000, ErrorMessage = "Укажите площадь")]
        public decimal Area { get; set; }

        public bool IsSharedSpace { get; set; } = false;
    }
}
