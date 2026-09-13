using Renty.Web.Models.Shared;

namespace Renty.Web.Models.Users
{
    // Данные для readonly-страницы профиля пользователя (Users/Profile.cshtml)
    public class UserProfileViewModel
    {
        public bool IsOwner { get; set; } // свой профиль -> показываем кнопку "Редактировать"

        public string AvatarUrl { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;

        public bool IsSuperHost { get; set; }
        public decimal Rating { get; set; }
        public int ReviewsCount { get; set; }
        public int MonthsOnPlatform { get; set; } // "N месяцев/лет на Airbnb"

        public bool IsVerified { get; set; }
        public string HomeCity { get; set; } = string.Empty;
        public string HomeCountry { get; set; } = string.Empty;
        public List<string> Languages { get; set; } = new();

        public string? Info { get; set; } // "Обо мне"

        public List<UserFactViewModel> Facts { get; set; } = new();

        public List<ReviewViewModel> Reviews { get; set; } = new();
    }
}
