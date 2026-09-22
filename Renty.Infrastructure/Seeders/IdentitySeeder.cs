using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Renty.Domain.Models.LookupsTables;
using Renty.Domain.Models.User;
using Renty.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Renty.Infrastructure.Seeders
{
    public static class IdentitySeeder
    {
        public static async Task SeedAdminAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        AppDbContext context) 
        {
            const string admin = "Admin";
            if (!await roleManager.RoleExistsAsync(admin))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(admin));
            }

            var adminEmail = "admin@renty.com";
            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

            if (existingAdmin == null)
            {

                var englishLang = await context.Languages.FirstOrDefaultAsync(l => l.Code == "en-en" || l.Name.Contains("English"));
                var russianLang = await context.Languages.FirstOrDefaultAsync(l => l.Code == "ru-ru" || l.Name.Contains("Russian"));
                var ukrainianLang = await context.Languages.FirstOrDefaultAsync(l => l.Code == "uk-uk" || l.Name.Contains("Ukrainian"));
                var frenchLang = await context.Languages.FirstOrDefaultAsync(l => l.Code == "fr-fr" || l.Name.Contains("French"));
                var spanishLang = await context.Languages.FirstOrDefaultAsync(l => l.Code == "es-es" || l.Name.Contains("Spanish"));

                var adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FirstName = "Алексей",
                    LastName = "Boss",
                    EmailConfirmed = true,
                    IsVerified = true,
                    IsSuperHost = true,
                    ResponseSpeed = "В течении часа",
                    Info = "Я создал Ренти во имя императора и для вас! Бронируйте жилища и обитайте в свое удовольствие",
                    AvatarUrl = "https://static.wikitide.net/1d6chanwiki/thumb/e/e5/Warhammer_-_Emperor_of_Mankind%2C_by_GENZOMAN.jpg/400px-Warhammer_-_Emperor_of_Mankind%2C_by_GENZOMAN.jpg"
                };


                if (englishLang != null) adminUser.Languages.Add(englishLang);
                if (russianLang != null) adminUser.Languages.Add(russianLang);
                if (ukrainianLang != null) adminUser.Languages.Add(ukrainianLang);
                if (frenchLang != null) adminUser.Languages.Add(frenchLang);
                if (spanishLang != null) adminUser.Languages.Add(spanishLang);

                var result = await userManager.CreateAsync(adminUser, "zfY8d4bKWjY!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, admin);
                }
                else
                {
                    var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Ошибки валидации Identity при создании администратора: {errorMessages}");
                }

                var adminFacts = new List<UserFact>
                {
                    new UserFact { UserId = adminUser.Id, Type = UserFactTypeEnum.Work, Value = "Любитель Warhammera" },
                    new UserFact { UserId = adminUser.Id, Type = UserFactTypeEnum.Pets, Value = "Верую в идеологию превосходства человечества" },
                    new UserFact { UserId = adminUser.Id, Type = UserFactTypeEnum.HowISpendTime, Value = "Люблю наших пользователей!" },
                    new UserFact { UserId = adminUser.Id, Type = UserFactTypeEnum.WhatILove, Value = "Позитивный и спокойный человек, но иногда душню" },
                    new UserFact { UserId = adminUser.Id, Type = UserFactTypeEnum.InterestingFact, Value = "Я свободно говорю на 6 языках и могу вести базовые разговоры еще на 21" },
                    new UserFact { UserId = adminUser.Id, Type = UserFactTypeEnum.WhereIWantToGo, Value = "Предпочитаю путшешествия с фотографиями. Никогда не знаешь где будет новая квартира!" }
                };

                foreach (var fact in adminFacts)
                {
                    context.UserFacts.Add(fact);
                }

                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedTestUsersAsync(
            UserManager<ApplicationUser> userManager,
            AppDbContext context)
        {
            var ukraine = await context.Countries.FirstOrDefaultAsync(c =>
            c.Name == "Ukraine" || c.NameRu == "Украина" || c.Name == "Украина");

            var kyiv = await context.Cities.FirstOrDefaultAsync(c =>
                c.Name == "Kyiv" || c.NameRu == "Киев" || c.Name == "Киев");

            var odesa = await context.Cities.FirstOrDefaultAsync(c =>
                c.Name == "Odesa" || c.Name == "Odessa" || c.NameRu == "Одесса" || c.Name == "Одесса");

            // Load languages
            var englishLang = await context.Languages.FirstOrDefaultAsync(l => l.Code == "en-en" || l.Name.Contains("English"));
            var russianLang = await context.Languages.FirstOrDefaultAsync(l => l.Code == "ru-ru" || l.Name.Contains("Russian"));
            var ukrainianLang = await context.Languages.FirstOrDefaultAsync(l => l.Code == "uk-uk" || l.Name.Contains("Ukrainian"));
            var frenchLang = await context.Languages.FirstOrDefaultAsync(l => l.Code == "fr-fr" || l.Name.Contains("French"));
            var spanishLang = await context.Languages.FirstOrDefaultAsync(l => l.Code == "es-es" || l.Name.Contains("Spanish"));

            // Проверяем, что локации существуют, иначе привязать пользователей не получится
            if (ukraine == null || kyiv == null || odesa == null)
            {
                Console.WriteLine("Ошибка сидирования пользователей: локации не найдены. Убедитесь, что CountrySeeder был запущен первым.");
                return;
            }

            // User 1: Изя Трофимивич - a cat lover from Odesa
            var izyaEmail = "laplas@renty.com";
            var izyaExisting = await userManager.FindByEmailAsync(izyaEmail);
            if (izyaExisting == null)
            {
                var izya = new ApplicationUser
                {
                    UserName = "izya-troff",
                    Email = izyaEmail,
                    FirstName = "Изя",
                    LastName = "Трофимивич",
                    EmailConfirmed = true,
                    IsVerified = true,
                    IsSuperHost = true,
                    HomeCountryId = ukraine.Id,
                    HomeCityId = odesa.Id,
                    TravelReason = "Исследование новых культур и встреча с местными жителями",
                    IsTravellingWithPet = true,
                    ResponseSpeed = "В течении 2 часов",
                    Info = "Привет! Я обожаю кошек и путешествия. Я люблю принимать гостей и делиться местными секретами о моем прекрасном городе. Моя квартира уютная и идеально подходит для тех, кто ищет аутентичный местный опыт.",
                    AvatarUrl = "https://headsupfortails.com/cdn/shop/articles/Welcoming_a_Cat_to_a_New_Home.jpg?v=1741258295"
                };

                await SeedUserAsync(userManager, izya, "zfY8d4bKWjY!");

                // Add facts for Izya
                var izyaFacts = new List<UserFact>
                {
                    new UserFact { UserId = izya.Id, Type = UserFactTypeEnum.Work, Value = "Графический дизайнер и веб-разработчик" },
                    new UserFact { UserId = izya.Id, Type = UserFactTypeEnum.Pets, Value = "Две рыжие кошки по имени Мурзик и Басья, и игривый попугай Карл" },
                    new UserFact { UserId = izya.Id, Type = UserFactTypeEnum.HowISpendTime, Value = "Создание веб-сайтов, рисование, исследование прибрежных кофеен, фотографирование моих кошек" },
                    new UserFact { UserId = izya.Id, Type = UserFactTypeEnum.WhatILove, Value = "Хороший кофе, свежие морепродукты, теплые летние ночи у моря, независимые фильмы" },
                    new UserFact { UserId = izya.Id, Type = UserFactTypeEnum.InterestingFact, Value = "Я свободно говорю на 4 языках и могу вести базовые разговоры еще на 3" },
                    new UserFact { UserId = izya.Id, Type = UserFactTypeEnum.WhereIWantToGo, Value = "Япония - особенно Киото и Токио, чтобы познакомиться с культурой и посетить кафе с кошками!" }
                };

                foreach (var fact in izyaFacts)
                {
                    context.UserFacts.Add(fact);
                }

                // Add languages for Izya
                if (englishLang != null)
                    izya.Languages.Add(englishLang);
                if (russianLang != null)
                    izya.Languages.Add(russianLang);
                if (ukrainianLang != null)
                    izya.Languages.Add(ukrainianLang);
                if (frenchLang != null)
                    izya.Languages.Add(frenchLang);

                await context.SaveChangesAsync();
            }

            var grettaEmail = "gretta@renty.com";
            var grettaExisting = await userManager.FindByEmailAsync(grettaEmail);
            if (grettaExisting == null)
            {
                var gretta = new ApplicationUser
                {
                    UserName = "gretta-user",
                    Email = grettaEmail,
                    FirstName = "Грета",
                    LastName = "Саацбаум",
                    EmailConfirmed = true,
                    IsVerified = true,
                    HomeCountryId = ukraine.Id,
                    HomeCityId = odesa.Id,
                    TravelReason = "Приключения и открытие скрытых жемчужин",
                    IsTravellingWithPet = false,
                    ResponseSpeed = "В течении 4 часов",
                    Info = "Привет! Я Грета, любопытный путешественник и фуд-блогер из Одессы. Я люблю делиться своей квартирой с другими путешественниками и давать персонализированные советы по изучению Одессы как местный житель. Давайте обменяемся историями о путешествиях!",
                    AvatarUrl = "https://thumb.wikimedia.org/wikipedia/en/thumb/2/25/Evanescence_-_Fallen.png/250px-Evanescence_-_Fallen.png?utm_source=en.wikipedia.org&utm_campaign=parser&utm_content=thumbnail"
                };

                await SeedUserAsync(userManager, gretta, "zfY8d4bKWjY!");

                // Add facts for Gretta
                var grettaFacts = new List<UserFact>
                {
                    new UserFact { UserId = gretta.Id, Type = UserFactTypeEnum.Work, Value = "Фуд-блогер и гид по кулинарным турам" },
                    new UserFact { UserId = gretta.Id, Type = UserFactTypeEnum.Generation, Value = "1990-е" },
                    new UserFact { UserId = gretta.Id, Type = UserFactTypeEnum.HowISpendTime, Value = "Фотография еды, посещение новых ресторанов, написание путеводителей, походы" },
                    new UserFact { UserId = gretta.Id, Type = UserFactTypeEnum.SchoolYears, Value = "Училась на журналиста в Одесском национальном университете" },
                    new UserFact { UserId = gretta.Id, Type = UserFactTypeEnum.WhatILove, Value = "Аутентичная местная кухня, уличная еда, винтажные рынки, встречи с людьми со всего мира" },
                    new UserFact { UserId = gretta.Id, Type = UserFactTypeEnum.UselessSkill, Value = "Я могу идеально имитировать звуки животных и заставлять людей смеяться" }
                };

                foreach (var fact in grettaFacts)
                {
                    context.UserFacts.Add(fact);
                }

                // Add languages for Gretta
                if (englishLang != null)
                    gretta.Languages.Add(englishLang);
                if (russianLang != null)
                    gretta.Languages.Add(russianLang);
                if (ukrainianLang != null)
                    gretta.Languages.Add(ukrainianLang);
                if (spanishLang != null)
                    gretta.Languages.Add(spanishLang);

                await context.SaveChangesAsync();
            }

            // User 3: Псайдак Даксон - a philosopher from Kyiv
            var psyduckEmail = "psyduck@renty.com";
            var psyduckExisting = await userManager.FindByEmailAsync(psyduckEmail);
            if (psyduckExisting == null)
            {
                var psyduck = new ApplicationUser
                {
                    UserName = "psyduck-user",
                    Email = psyduckEmail,
                    FirstName = "Псайдак",
                    LastName = "Даксон",
                    EmailConfirmed = true,
                    IsVerified = false,
                    HomeCountryId = ukraine.Id,
                    HomeCityId = kyiv.Id,
                    TravelReason = "Интеллектуальный обмен и культурное погружение",
                    IsTravellingWithPet = false,
                    ResponseSpeed = "Разное",
                    Info = "Привет! Я студент философии и писатель из Киева. Моя квартира маленькая, но заполнена книгами и имеет отличный балкон с видом на город. Я заинтересован в приеме любопытных путешественников, которые любят глубокие разговоры о жизни и культуре.",
                    AvatarUrl = "https://static.wikia.nocookie.net/pokemon/images/3/3f/0054Psyduck.png/revision/latest?cb=20260616122621&path-prefix=ru"
                };

                await SeedUserAsync(userManager, psyduck, "zfY8d4bKWjY!");

                // Add facts for Psyduck
                var psyduckFacts = new List<UserFact>
                {
                    new UserFact { UserId = psyduck.Id, Type = UserFactTypeEnum.Work, Value = "Студент философии и внештатный писатель" },
                    new UserFact { UserId = psyduck.Id, Type = UserFactTypeEnum.Generation, Value = "1998-е" },
                    new UserFact { UserId = psyduck.Id, Type = UserFactTypeEnum.HowISpendTime, Value = "Чтение философских книг, написание эссе, посещение музеев, долгие прогулки по Киеву, посещение литературных кафе" },
                    new UserFact { UserId = psyduck.Id, Type = UserFactTypeEnum.SchoolYears, Value = "Учился в Киевском национальном университете имени Тараса Шевченко" },
                    new UserFact { UserId = psyduck.Id, Type = UserFactTypeEnum.InterestingFact, Value = "Я прочитал все произведения Достоевского и люблю обсуждать экзистенциализм" },
                    new UserFact { UserId = psyduck.Id, Type = UserFactTypeEnum.LifeStory, Value = "Парадокс странника: поиск истины через путешествия" }
                };

                foreach (var fact in psyduckFacts)
                {
                    context.UserFacts.Add(fact);
                }

                // Add languages for Psyduck
                if (englishLang != null)
                    psyduck.Languages.Add(englishLang);
                if (russianLang != null)
                    psyduck.Languages.Add(russianLang);
                if (ukrainianLang != null)
                    psyduck.Languages.Add(ukrainianLang);
                if (frenchLang != null)
                    psyduck.Languages.Add(frenchLang);

                await context.SaveChangesAsync();
            }
        }

        private static async Task<bool> SeedUserAsync(UserManager<ApplicationUser> userManager, ApplicationUser user, string password)
        {
            var existingUser = await userManager.FindByEmailAsync(user.Email);
            if (existingUser != null)
            {
                throw new ArgumentException("Пользователь уже существует", nameof(user.Email));
            }

            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Ошибки валидации Identity: {errorMessages}");
            }

            return true;
        }
    }
}
