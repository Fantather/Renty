using Renty.Domain.Models.LookupsTables;

namespace Renty.Web.Models.Users
{
    // Один факт о пользователе (работа, поколение, любимая песня и т.п.) — иконка+подпись+значение
    public class UserFactViewModel
    {
        public UserFactTypeEnum Type { get; set; }
        public string? Value { get; set; } = string.Empty;
        public string IconName { get; set; } = string.Empty;
    }
}
