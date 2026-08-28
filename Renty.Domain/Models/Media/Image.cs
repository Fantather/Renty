using Renty.Domain.Models.LookupsTables;
using System.ComponentModel.DataAnnotations.Schema;

namespace Renty.Domain.Models.Media
{
    public abstract class Image
    {
        public Guid Id { get; set; }

        // URL изображения (путь к файлу, никто не хранит изображения в бд)
        public string ImageUrl { get; set; } = string.Empty;

        // Название изображения
        public string Title { get; set; } = "Title Image";

        // Описание изображения, может быть пустым
        public string? Description { get; set; }

        //// Порядок отображения, я закоментировала потому что не уверенна нужен ли он
        //public int DisplayOrder { get; set; } = 0;

        // Является ли изображение главным
        public bool IsPrimary { get; set; } = false;

        public int? ImageTypeId { get; set; }
        [ForeignKey(nameof(ImageTypeId))]
        public virtual ImageType? ImageType { get; set; }

        // Дата загрузки
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
