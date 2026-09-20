using System.ComponentModel.DataAnnotations;

namespace Renty.Web.Models.InputModels.Properties
{
    public class TitleInputModel
    {
        [Required(ErrorMessage = "Название обязательно")]
        [StringLength(100, ErrorMessage = "Название должно быть не длиннее 100 символов")]
        public string Name { get; set; } = string.Empty;
    }
}
