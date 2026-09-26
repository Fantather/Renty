using System.ComponentModel.DataAnnotations;

namespace Renty.Web.Models.InputModels.Properties
{
    public class AddressInputModel
    {
        public string? PlaceId { get; set; }

        public string? Address { get; set; }

        public string? Street { get; set; }

        public string? District { get; set; }

        [Required(ErrorMessage = "Город обязателен")]
        public string? CityName { get; set; }

        [Required(ErrorMessage = "Страна обязательна")]
        public string? CountryName { get; set; }
    }
}
