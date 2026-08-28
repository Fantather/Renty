using Slugify;
using Renty.Domain.Interfaces;

namespace Renty.Infrastructure.Helpers
{
    public static class SlugGenerator
    {
        private static readonly SlugHelper _slugHelper = CreateConfiguredSlugHelper();

        private static SlugHelper CreateConfiguredSlugHelper()
        {
            var config = new SlugHelperConfiguration();

            var transliterationMap = new Dictionary<string, string>
            {
                {"а", "a"}, {"б", "b"}, {"в", "v"}, {"г", "g"}, {"д", "d"},
                {"е", "e"}, {"ё", "yo"}, {"ж", "zh"}, {"з", "z"}, {"и", "i"},
                {"й", "y"}, {"к", "k"}, {"л", "l"}, {"м", "m"}, {"н", "n"},
                {"о", "o"}, {"п", "p"}, {"р", "r"}, {"с", "s"}, {"т", "t"},
                {"у", "u"}, {"ф", "f"}, {"х", "h"}, {"ц", "ts"}, {"ч", "ch"},
                {"ш", "sh"}, {"щ", "shch"}, {"ъ", ""}, {"ы", "y"}, {"ь", ""},
                {"э", "e"}, {"ю", "yu"}, {"я", "ya"},

                // 
                {"і", "i"}, {"ї", "yi"}, {"є", "ye"}, {"ґ", "g"}
            };

            foreach (var pair in transliterationMap)
            {


                config.StringReplacements.Add(pair.Key, pair.Value);
                config.StringReplacements.Add(pair.Key.ToUpperInvariant(), pair.Value);
            }

            return new SlugHelper(config);
        }

        /// <summary>
        /// Generates a URL-friendly slug from the specified text, optionally appending a unique identifier.
        /// </summary>
        public static string GenerateSlug(string sourceText, Guid? appendUniqueId = null)
        {
            if (string.IsNullOrWhiteSpace(sourceText))
            {
                return appendUniqueId?.ToString("N") ?? Guid.NewGuid().ToString("N");
            }

            // Передаем исходный текст в преднастроенный хелпер
            var baseSlug = _slugHelper.GenerateSlug(sourceText);

            if (appendUniqueId.HasValue)
            {
                var shortId = appendUniqueId.Value.ToString("N")[^6..];
                return $"{baseSlug}-{shortId}";
            }

            return baseSlug;
        }

        /// <summary>
        /// Generates a unique SKU (Stock Keeping Unit).
        /// </summary>
        public static string GenerateSku(string prefix = "ITEM")
        {
            var randomString = Guid.NewGuid().ToString("N").ToUpper();
            return $"{prefix}-{randomString.Substring(0, 4)}-{randomString.Substring(4, 4)}";
        }

    }
}
