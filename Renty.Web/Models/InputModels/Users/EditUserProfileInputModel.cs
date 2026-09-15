using System.ComponentModel.DataAnnotations;

namespace Renty.Web.Models.InputModels.Users
{
    public class EditUserProfileInputModel
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public string? AvatarUrl { get; set; }

        public Guid? HomeCityId { get; set; }
        public string? HomeCityDisplay { get; set; } // "Алмере, Нидерланды" — показать в поле поиска города

        public List<Guid> LanguageIds { get; set; } = new();

        [MaxLength(500)]
        public string? Info { get; set; }

        public List<UserFactInputModel> Facts { get; set; } = new();

        public bool ShowGeneration { get; set; } = true; // "Показывать десятилетие моего рождения"
    }
}
