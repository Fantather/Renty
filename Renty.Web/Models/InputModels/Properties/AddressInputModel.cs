using System.ComponentModel.DataAnnotations;

namespace Renty.Web.Models.InputModels.Properties
{
    public class AddressInputModel
    {
        public string? PlaceId { get; set; }

        [Required(ErrorMessage = "Адрес обязателен")]
        public string Address { get; set; } = string.Empty;

        public string? Street { get; set; }

        public string? District { get; set; }

        public string? CityName { get; set; }

        public string? CountryName { get; set; }
    }
}
