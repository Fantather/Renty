using System.ComponentModel.DataAnnotations;

namespace Renty.Web.Models.InputModels.Properties
{
    /// <summary>
    /// Основная модель для создания нового объявления
    /// </summary>
    /// <remarks>
    /// Важно возвоащать айди квартиры что бы подгрузить потом фотографии
    /// </remarks>
    public class PropertyInputModel
    {
        // Базовая информация
        [Required(ErrorMessage = "Название обязательно")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Название должно быть от 10 до 100 символов")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Описание обязательно")]
        [MaxLength(2000, ErrorMessage = "Описание слишком длинное")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Выберите категорию жилья")]
        public Guid CategoryId { get; set; }

        //  Локация
        [Required(ErrorMessage = "Страна обязательна")]
        public Guid CountryId { get; set; }

        [Required(ErrorMessage = "Город обязателен")]
        public Guid CityId { get; set; }

        [Required(ErrorMessage = "Адрес обязателен")]
        public string Address { get; set; } = string.Empty;

        public string? Street { get; set; }

        public string? District { get; set; }

        // Координаты для карты
        [Range(-90, 90)]
        public double Latitude { get; set; }
        [Range(-180, 180)]
        public double Longitude { get; set; }

        // денежная информация
        [Required(ErrorMessage = "Укажите цену за ночь")]
        [Range(1, 1000000, ErrorMessage = "Цена должна быть больше нуля")]
        public decimal PricePerNight { get; set; }

        public string Currency { get; set; } = "UAH";

        public TimeSpan? CheckInTime { get; set; }
        public TimeSpan? CheckOutTime { get; set; }

        public string? HouseRules { get; set; }

        // Заполняем что есть в нашей квартирке. Так как я не привязівала комнаті к прям созданию типа комнаті, то оно так
        [Range(1, 50, ErrorMessage = "Количество гостей должно быть от 1 до 50")]
        public int MaxGuests { get; set; }
        public int BedsCount { get; set; }
        public int BedroomsCount { get; set; }
        public int BathroomsCount { get; set; }
        public int? Floor { get; set; }
        public int? FloorsCount { get; set; }

        // удобства
        public List<Guid> AmenityIds { get; set; } = new();

        //теги квартиры .\Renty.Domain\Models\Properties\PropertyTag.cs
        public List<Guid> TagIds { get; set; } = new();

        // комнаты создаются отдельно, тут необязательны
        public List<CreateRoomInputModel> Rooms { get; set; } = new();

    }
}
