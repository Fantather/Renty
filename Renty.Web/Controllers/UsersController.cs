using Microsoft.AspNetCore.Mvc;
using Renty.Domain.Models.LookupsTables;
using Renty.Web.Extensions;
using Renty.Web.Models.InputModels.Users;
using Renty.Web.Models.Shared;
using Renty.Web.Models.Users;

namespace Renty.Web.Controllers
{
    public class UsersController : Controller
    {
        // TEMPORARY: заглушка вместо реального GetUserProfileQuery — контроллер/DI на бэке ещё не готовы.
        // Когда будут готовы, тело метода заменится на вызов _mediator.Send(new GetUserProfileQuery(id)),
        // а ViewModel и Profile.cshtml трогать не придётся.
        public IActionResult Profile(Guid? id)
        {
            var vm = new UserProfileViewModel
            {
                IsOwner = !id.HasValue,
                AvatarUrl = "https://placehold.co/160x160",
                FullName = "Алексей",
                IsSuperHost = true,
                Rating = 4.92m,
                ReviewsCount = 110,
                MonthsOnPlatform = 10,
                IsVerified = true,
                HomeCity = "Алмере",
                HomeCountry = "Нидерланды",
                Languages = new List<string> { "Английский", "Русский" },
                Info = "Зашёл в бар как-то чёрный сталкер",
                Facts = new List<UserFactViewModel>
                {
                    new() { Type = UserFactTypeEnum.Work, Value = "Работа", IconName = UserFactTypeEnum.Work.GetMeta().IconName },
                    new() { Type = UserFactTypeEnum.Generation, Value = "00-е", IconName = UserFactTypeEnum.Generation.GetMeta().IconName },
                    new() { Type = UserFactTypeEnum.HowISpendTime, Value = "Делаю часами", IconName = UserFactTypeEnum.HowISpendTime.GetMeta().IconName },
                    new() { Type = UserFactTypeEnum.Pets, Value = "Питомцы", IconName = UserFactTypeEnum.Pets.GetMeta().IconName },
                    new() { Type = UserFactTypeEnum.SchoolYears, Value = "Я устал босс (Академия ШАГ)", IconName = UserFactTypeEnum.SchoolYears.GetMeta().IconName },
                    new() { Type = UserFactTypeEnum.FavoriteSchoolSong, Value = "Любимая песня", IconName = UserFactTypeEnum.FavoriteSchoolSong.GetMeta().IconName },
                    new() { Type = UserFactTypeEnum.InterestingFact, Value = "Я не ем людей", IconName = UserFactTypeEnum.InterestingFact.GetMeta().IconName },
                    new() { Type = UserFactTypeEnum.UselessSkill, Value = "Навык", IconName = UserFactTypeEnum.UselessSkill.GetMeta().IconName },
                    new() { Type = UserFactTypeEnum.LifeStory, Value = "Я говорил что я не ем людей? Так вот...", IconName = UserFactTypeEnum.LifeStory.GetMeta().IconName },
                    new() { Type = UserFactTypeEnum.WhatILove, Value = "Говорить что я не ем людей", IconName = UserFactTypeEnum.WhatILove.GetMeta().IconName },
                    new() { Type = UserFactTypeEnum.WhereIWantToGo, Value = "На экзопланете", IconName = UserFactTypeEnum.WhereIWantToGo.GetMeta().IconName },
                },
                Reviews = new List<ReviewViewModel>
                {
                    new() { AuthorName = "Cosima Therese", AuthorAvatarUrl = "https://placehold.co/60x60", Rating = 5, Text = "Мы сразу почувствовали себя очень комфортно в этом жилье, как дома.", CreatedAt = DateTime.UtcNow.AddDays(-6) },
                    new() { AuthorName = "Christian", AuthorAvatarUrl = "https://placehold.co/60x60", Rating = 5, Text = "Красивый дом в нашем районе, где моя семья остановилась во время поездки в Париж.", CreatedAt = DateTime.UtcNow.AddDays(-14) },
                    new() { AuthorName = "Omar", AuthorAvatarUrl = "https://placehold.co/60x60", Rating = 5, Text = "Очень хорошо. Мы смогли осмотреть достопримечательности и провести несколько дней.", CreatedAt = DateTime.UtcNow.AddDays(-21) },
                },
            };

            return View(vm);
        }

        // TEMPORARY: заглушка вместо реального GetUserProfileQuery/UpdateUserProfileCommand — бэк ещё не готов.
        [HttpGet]
        public IActionResult Edit()
        {
            var vm = new EditUserProfileViewModel
            {
                Input = new EditUserProfileInputModel
                {
                    FirstName = "Алексей",
                    LastName = "",
                    AvatarUrl = null,
                    HomeCityDisplay = "Алмере, Нидерланды",
                    LanguageIds = new List<Guid> { EnglishLanguageId, RussianLanguageId },
                    Info = "Зашёл в бар как-то чёрный сталкер",
                    Facts = new List<UserFactInputModel>
                    {
                        new() { Type = UserFactTypeEnum.Work, Value = "Работа", IconName = UserFactTypeEnum.Work.GetMeta().IconName },
                        new() { Type = UserFactTypeEnum.Generation, Value = "00-е", IconName = UserFactTypeEnum.Generation.GetMeta().IconName },
                        new() { Type = UserFactTypeEnum.HowISpendTime, Value = "Делаю часами", IconName = UserFactTypeEnum.HowISpendTime.GetMeta().IconName },
                        new() { Type = UserFactTypeEnum.Pets, Value = "Питомцы", IconName = UserFactTypeEnum.Pets.GetMeta().IconName },
                        new() { Type = UserFactTypeEnum.SchoolYears, Value = "Я устал босс (Академия ШАГ)", IconName = UserFactTypeEnum.SchoolYears.GetMeta().IconName },
                        new() { Type = UserFactTypeEnum.FavoriteSchoolSong, Value = "Любимая песня", IconName = UserFactTypeEnum.FavoriteSchoolSong.GetMeta().IconName },
                        new() { Type = UserFactTypeEnum.InterestingFact, Value = "Я не ем людей", IconName = UserFactTypeEnum.InterestingFact.GetMeta().IconName },
                        new() { Type = UserFactTypeEnum.UselessSkill, Value = "Навык", IconName = UserFactTypeEnum.UselessSkill.GetMeta().IconName },
                        new() { Type = UserFactTypeEnum.LifeStory, Value = "Я говорил что я не ем людей? Так вот...", IconName = UserFactTypeEnum.LifeStory.GetMeta().IconName },
                        new() { Type = UserFactTypeEnum.WhatILove, Value = "Говорить что я не ем людей", IconName = UserFactTypeEnum.WhatILove.GetMeta().IconName },
                        new() { Type = UserFactTypeEnum.WhereIWantToGo, Value = "На экзопланете", IconName = UserFactTypeEnum.WhereIWantToGo.GetMeta().IconName },
                    },
                },
                AvailableLanguages = new List<LanguageOptionViewModel>
                {
                    new() { Id = EnglishLanguageId, Name = "Английский" },
                    new() { Id = RussianLanguageId, Name = "Русский" },
                    new() { Id = Guid.NewGuid(), Name = "Украинский" },
                    new() { Id = Guid.NewGuid(), Name = "Испанский" },
                    new() { Id = Guid.NewGuid(), Name = "Французский" },
                    new() { Id = Guid.NewGuid(), Name = "Немецкий" },
                },
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(EditUserProfileInputModel model)
        {
            return RedirectToAction(nameof(Profile));
        }

        private static readonly Guid EnglishLanguageId = Guid.NewGuid();
        private static readonly Guid RussianLanguageId = Guid.NewGuid();
    }
}
