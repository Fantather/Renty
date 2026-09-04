using Renty.Domain.Models.LookupsTables;
using System.ComponentModel.DataAnnotations.Schema;

namespace Renty.Domain.Models.Media
{
    public abstract class Image
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();

        // URL изображения (путь к файлу, никто не хранит изображения в бд)
        public string ImageUrl { get; set; } = string.Empty;

        // Название изображения
        public string Title { get; set; } = "Title Image";

        // Описание изображения, может быть пустым
        public string? Description { get; set; }

        //// Порядок отображения, я закоментировала потому что не уверенна нужен ли он
        public int DisplayOrder { get; set; } = 0;

        // Является ли изображение главным
        public bool IsPrimary { get; set; } = false;

        // Дата загрузки
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
