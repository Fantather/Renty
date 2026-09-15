namespace Renty.Web.Models.Shared
{
    // Один язык из справочника Languages для чекбоксов на форме редактирования профиля
    public class LanguageOptionViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
