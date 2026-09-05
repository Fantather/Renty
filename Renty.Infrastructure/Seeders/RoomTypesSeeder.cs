using Microsoft.EntityFrameworkCore;
using Renty.Domain.Models.Properties;
using Renty.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace Renty.Infrastructure.Seeders
{
    public static class RoomTypesSeeder
    {
        private static readonly RoomType[] roomTypes = {

            // Основные жилые зоны
            new RoomType
            {
                Name = "Спальня",
                Description = "Изолированная комната со спальным местом (кроватью или диваном) для сна и личного отдыха",
                IsActive = true
            },
            new RoomType
            {
                Name = "Гостиная",
                Description = "Общая комната для совместного отдыха, просмотра ТВ и общения гостей",
                IsActive = true
            },
            new RoomType
            {
                Name = "Студия",
                Description = "Единое открытое жилое пространство, объединяющее зоны сна, отдыха и кухни",
                IsActive = true
            },
            new RoomType
            {
                Name = "Детская комната",
                Description = "Спальня, оборудованная детскими или двухъярусными кроватями и игровой зоной",
                IsActive = true
            },

            // Санитарные и кухонные зоны
            new RoomType
            {
                Name = "Ванная комната",
                Description = "Санитарный узел с ванной или душевой кабиной, раковиной и туалетом",
                IsActive = true
            },
            new RoomType
            {
                Name = "Туалет",
                Description = "Отдельный гостевой санузел без душевой зоны",
                IsActive = true
            },
            new RoomType
            {
                Name = "Кухня",
                Description = "Зона приготовления пищи, оснащенная плитой, холодильником и бытовой техникой",
                IsActive = true
            },
            new RoomType
            {
                Name = "Столовая",
                Description = "Выделенное пространство с обеденным столом для совместных приемов пищи",
                IsActive = true
            },

            // Работа и сервисные зоны
            new RoomType
            {
                Name = "Рабочий кабинет",
                Description = "Изолированное тихое пространство со столом и креслом для комфортной удаленной работы",
                IsActive = true
            },
            new RoomType
            {
                Name = "Прихожая",
                Description = "Входная зона жилья со шкафом или вешалкой для верхней одежды и обуви",
                IsActive = true
            },
            new RoomType
            {
                Name = "Прачечная",
                Description = "Хозяйственное помещение со стиральной машиной, сушилкой и гладильными принадлежностями",
                IsActive = true
            },

            // Открытые и рекреационные пространства
            new RoomType
            {
                Name = "Балкон / Терраса",
                Description = "Открытая или застекленная площадка на свежем воздухе с уличной мебелью",
                IsActive = true
            },
            new RoomType
            {
                Name = "Внутренний двор / Патио",
                Description = "Придомовая территория, сад или открытая площадка с зоной отдыха или барбекю",
                IsActive = true
            },
            new RoomType
            {
                Name = "Сауна / Баня",
                Description = "Парная зона для релаксации (часто в загородных коттеджах или премиум-апартаментах)",
                IsActive = true
            },
            new RoomType
            {
                Name = "Спортивный зал",
                Description = "Помещение с тренажерами и спортивным инвентарем",
                IsActive = true
            }
            };

        public static async Task SeedAsync(AppDbContext context)
        {
            if (await context.RoomType.AnyAsync())
            {
                return;
            }

            foreach (var roomType in roomTypes)
            {
                try
                {
                    if (roomType.Id == Guid.Empty)
                    {
                        roomType.Id = Guid.CreateVersion7();
                    }

                    await SeedRoomTypeAsync(context, roomType);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при добавлении типа комнаты '{roomType.Name}': {ex.Message}");
                }
            }
        }

        private static async Task<bool> SeedRoomTypeAsync(AppDbContext context, RoomType roomType)
        {
            if (await context.RoomType.AnyAsync(rt => rt.Name == roomType.Name))
            {
                throw new ArgumentException("Тип комнаты уже существует", nameof(roomType.Name));
            }

            await context.RoomType.AddAsync(roomType);
            await context.SaveChangesAsync();
            return true;
        }
    }
}