using Renty.Web.Models.InputModels.Properties;
using System.ComponentModel.DataAnnotations;

namespace Renty.Web.Models.InputModels.Media
{
    /// <summary>
    /// вот тут просто с фронта загрузить кучу фото
    /// </summary>
    public class UploadPropertyImagesInputModel
    {
        // Уже сохраненные фото
        public List<ExistingImageInputModel> ExistingImages { get; set; } = new();

        [Required(ErrorMessage = "Выберите хотя бы одно фото")]
        //формат файов что был отправлен через  форму. Просто сперва загрузили
        public List<IFormFile> Images { get; set; } = new();

        // Финальный порядок фото - собирается перед отправкой
        public List<OrderedImageRefInputModel> OrderedImages { get; set; } = new();
    }
    public class ExistingImageInputModel
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
    public class OrderedImageRefInputModel
    {
        public string Type { get; set; } = null!; // "Existing" | "New"
        public Guid? Id { get; set; }
        public int? FileIndex { get; set; }
    }
}
