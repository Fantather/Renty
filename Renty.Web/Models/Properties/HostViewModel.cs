namespace Renty.Web.Models.Properties
{
    // Хозяин квартиры
    public class HostViewModel
    {
        public Guid Id { get; set; }
        public string? AvatarUrl { get; set; }
        public string FullName { get; set; } = string.Empty;
        public bool IsSuperhost { get; set; } // значок "Суперхозяин"
        public bool IsVerified { get; set; }
        public string ResponseSpeed { get; set; } = string.Empty; // "Отвечает в течение часа" и т.п.
        public int YearsHosting { get; set; } // сколько лет принимает гостей
    }
}
