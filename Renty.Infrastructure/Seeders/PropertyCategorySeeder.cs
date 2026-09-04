using Microsoft.EntityFrameworkCore;
using Renty.Domain.Models;
using Renty.Infrastructure.Data;
using Renty.Infrastructure.Helpers;

//public abstract class Category : IHasSlug
//{
//    public Guid Id { get; set; } = Guid.CreateVersion7();

//    public string Slug { get; set; } = string.Empty;
//    public string Name { get; set; } = string.Empty;

//    public string? Description { get; set; }

//    // URL картинки категории
//    public string? ImageUrl { get; set; }

//    // Активна ли категория
//    public bool IsActive { get; set; } = true;

//    // Даты
//    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

//    public DateTime? UpdatedAt { get; set; }
//}
//}

namespace Renty.Infrastructure.Seeders
{
    public static class PropertyCategorySeeder
    {
        private static readonly PropertiesCategory[] categories = {
            new PropertiesCategory {
                Name = "Красивые виды",
                Description = "Жилье с панорамными или живописными видами",
                Slug = "", IsActive = true, ImageUrl = "/icons/property-category/ri--home-4-line.svg"
            },
            new PropertiesCategory {
                Name = "Маленькие квартиры",
                Description = "Компактное и уютное жилье для одного или двух человек",
                Slug = "", IsActive = true, ImageUrl = "/icons/property-category/ri--home-3-line.svg"
            },
            new PropertiesCategory {
                Name = "Большие квартиры",
                Description = "Просторное жилье для больших семей или компаний",
                Slug = "", IsActive = true, ImageUrl = "/icons/property-category/ri--home-4-line.svg"
            },
            new PropertiesCategory {
                Name = "Комнаты",
                Description = "Отдельные комнаты в аренду в квартирах или домах",
                Slug = "", IsActive = true, ImageUrl = "/icons/property-category/ri--home-4-line.svg"
            },
            new PropertiesCategory {
                Name = "Хостелы",
                Description = "Бюджетные спальные места в общих номерах",
                Slug = "", IsActive = true, ImageUrl = "/icons/property-category/ri--home-4-line.svg"
            },
            new PropertiesCategory {
                Name = "Люкс",
                Description = "Элитное жилье премиум-класса с высоким уровнем комфорта",
                Slug = "", IsActive = true, ImageUrl = "/icons/property-category/ri--home-4-line.svg"
            },
            new PropertiesCategory {
                Name = "В центре города",
                Description = "Жилье в самом сердце города в пешей доступности от достопримечательностей",
                Slug = "", IsActive = true, ImageUrl = "/icons/property-category/ri--home-4-line.svg"
            },
            new PropertiesCategory {
                Name = "Сельская местность",
                Description = "Спокойный отдых в загородных домах, деревнях или на фермах",
                Slug = "", IsActive = true, ImageUrl = "/icons/property-category/ri--home-4-line.svg"
            },
            new PropertiesCategory {
                Name = "Дизайнерское жилье",
                Description = "Апартаменты с уникальным интерьером от профессиональных дизайнеров",
                Slug = "", IsActive = true, ImageUrl = "/icons/property-category/ri--home-4-line.svg"
            },
            new PropertiesCategory {
                Name = "У моря",
                Description = "Жилье на первой линии или в шаговой доступности от пляжа",
                Slug = "", IsActive = true, ImageUrl = "/icons/property-category/ri--home-4-line.svg"
            },
            new PropertiesCategory {
                Name = "Особняки",
                Description = "Роскошные отдельно стоящие дома и большие резиденции",
                Slug = "", IsActive = true, ImageUrl = "/icons/property-category/ri--home-4-line.svg"
            }

        };
        public static async Task SeedAsync(AppDbContext context)
        {
            if (await context.PropertiesCategory.AnyAsync())
            {
                return;
            }

            foreach (var category in categories)
            {
                try
                {
                    if (category.Id == Guid.Empty)
                    {
                        category.Id = Guid.CreateVersion7();
                    }
                    if (string.IsNullOrWhiteSpace(category.Slug))
                    {
                        category.Slug = SlugGenerator.GenerateSlug(category.Name);
                    }
                    await SeedCategoryAsync(context, category);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при добавлении категории '{category.Name}': {ex.Message}");
                }
            }
        }

        private static async Task<bool> SeedCategoryAsync(AppDbContext context, PropertiesCategory category)
        {
            if (await context.PropertiesCategory.AnyAsync(c => c.Name == category.Name))
            {
                throw new ArgumentException("Категория уже существует", nameof(category.Name));
            }

            await context.PropertiesCategory.AddAsync(category);
            await context.SaveChangesAsync();
            return true;
        }
    }
}