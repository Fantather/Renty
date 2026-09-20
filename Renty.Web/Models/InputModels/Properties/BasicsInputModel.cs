using System.ComponentModel.DataAnnotations;

namespace Renty.Web.Models.InputModels.Properties
{
    public class BasicsInputModel
    {
        [Range(1, 50, ErrorMessage = "Количество гостей должно быть от 1 до 50")]
        public int MaxGuests { get; set; }
        public int BedsCount { get; set; }
        public int BedroomsCount { get; set; }
        public int BathroomsCount { get; set; }
    }
}
