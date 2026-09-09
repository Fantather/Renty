using Microsoft.EntityFrameworkCore;
using Renty.Domain.Models.LookupsTables;
using Renty.Infrastructure.Data;

namespace Renty.Infrastructure.Seeders
{
    public static class LanguagesSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (!await context.Languages.AnyAsync())
            {
                var defaultLanguages = new Dictionary<string, string>
                {
                    { "English", "en" },
                    { "Українська", "uk" },
                    { "Polski", "pl" },
                    { "Español", "es" },
                    { "Deutsch", "de" },
                    { "Français", "fr" }
                };

                foreach (var lang in defaultLanguages)
                {
                    await SeedLanguageAsync(context, lang.Key, lang.Value);
                }
            }
        }

        private static async Task<Languages> SeedLanguageAsync(AppDbContext context, string languageName, string languageCode)
        {
            var existingLanguage = await context.Languages.FirstOrDefaultAsync(l => l.Code == languageCode);

            if (existingLanguage != null)
            {
                return existingLanguage;
            }

            var language = new Languages
            {
                Id = Guid.CreateVersion7(),
                Name = languageName,
                Code = languageCode
            };

            await context.Languages.AddAsync(language);
            await context.SaveChangesAsync();

            return language;
        }
    }
}