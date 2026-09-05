using Microsoft.EntityFrameworkCore;
using Renty.Domain.Models.Properties;
using Renty.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace Renty.Infrastructure.Seeders
{
    public static class TagSeeder
    {
        private static readonly Tag[] tags = {
        new Tag { Name = "Можно с питомцами", Description = "Можно с питомцами", DisplayOrder = 1, IsActive = true },
        new Tag { Name = "Для детей", Description = "Отлично подходит для семей с детьми", DisplayOrder = 2, IsActive = true },
        new Tag { Name = "Удаленная работа", Description = "Есть рабочее место и быстрый интернет", DisplayOrder = 3, IsActive = true },
        new Tag { Name = "Эко-жилье", Description = "Экологичное жилье", DisplayOrder = 4, IsActive = true },
        new Tag { Name = "Премиум", Description = "Жилье премиум-класса с эксклюзивным дизайном", DisplayOrder = 5, IsActive = true },
        new Tag { Name = "У моря", Description = "В шаговой доступности от пляжа", DisplayOrder = 6, IsActive = true },
        new Tag { Name = "Тихое место", Description = "Спокойное место без шумных дорог и вечеринок", DisplayOrder = 7, IsActive = true }
        };

        public static async Task SeedAsync(AppDbContext context)
        {
            if (await context.Tags.AnyAsync())
            {
                return;
            }

            foreach (var tag in tags)
            {
                try
                {
                    if (tag.Id == Guid.Empty)
                    {
                        tag.Id = Guid.CreateVersion7();
                    }

                    await SeedTagAsync(context, tag);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при добавлении тега '{tag.Name}': {ex.Message}");
                }
            }
        }

        private static async Task<bool> SeedTagAsync(AppDbContext context, Tag tag)
        {
            if (await context.Tags.AnyAsync(t => t.Name == tag.Name))
            {
                throw new ArgumentException("Тег уже существует", nameof(tag.Name));
            }

            await context.Tags.AddAsync(tag);
            await context.SaveChangesAsync();
            return true;
        }
    }
}