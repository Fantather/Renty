using System.ComponentModel.DataAnnotations;

namespace Renty.Web.Models.InputModels.Properties
{
    public class CategoryInputModel
    {
        [Required(ErrorMessage = "Выберите категорию жилья")]
        public Guid CategoryId { get; set; }
    }
}
