using System.ComponentModel.DataAnnotations;

namespace Renty.Web.Models.Lelik.Media
{
    /// <summary>
    /// вот тут просто с фронта загрузить кучу фото
    /// </summary>  
    public class UploadPropertyImagesInputModel
    {
        [Required]
        public Guid PropertyId { get; set; }

        [Required(ErrorMessage = "Выберите хотя бы одно фото")]
        //формат файов что был отправлен через  форму. Просто сперва загрузили
        public List<IFormFile> Images { get; set; } = new();
}
}
