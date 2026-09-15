namespace Renty.Web.Models.Shared
{
    // Аватарка пользователя: фото, если есть, иначе первая буква имени на цветном фоне
    public class AvatarViewModel
    {
        public string? Url { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SizePx { get; set; } = 64;
    }
}
