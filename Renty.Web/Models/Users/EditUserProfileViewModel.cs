using Renty.Web.Models.InputModels.Users;
using Renty.Web.Models.Shared;

namespace Renty.Web.Models.Users
{
    // Данные для страницы редактирования профиля (Users/Edit.cshtml)
    public class EditUserProfileViewModel
    {
        public EditUserProfileInputModel Input { get; set; } = new();
        public List<LanguageOptionViewModel> AvailableLanguages { get; set; } = new();  // Все возможные языки
    }
}
