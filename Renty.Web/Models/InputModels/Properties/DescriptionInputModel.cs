using System.ComponentModel.DataAnnotations;

namespace Renty.Web.Models.InputModels.Properties
{
    public class DescriptionInputModel
    {
        [Required(ErrorMessage = "Описание обязательно")]
        [MaxLength(2000, ErrorMessage = "Описание слишком длинное")]
        public string Description { get; set; } = string.Empty;
    }
}
