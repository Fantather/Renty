using Microsoft.EntityFrameworkCore;
using Renty.Infrastructure.Data;
using Renty.Domain.Models.Properties.Anemities;
namespace Renty.Infrastructure.Seeders
{
    public static class AmenitiesSeeder
    {
        private static readonly Anemities[] amenities = {
        new Anemities { Name = "Wi-Fi", Description = "Бесплатный Wi-Fi", IconUrl = "/icons/amenities/bi--wifi.svg", IsActive = true },
        new Anemities { Name = "Парковка", Description = "Бесплатная парковка", IconUrl = "/icons/amenities/bxs--parking.svg", IsActive = true },
        new Anemities { Name = "Укомплектованная кухня", Description = "Кухня со всеми необходимыми приборами и плитой", IconUrl = "/icons/amenities/guidance--kitchen.svg", IsActive = true },
        new Anemities { Name = "Кухня", Description = "Базовая кухня в наличии", IconUrl = "/icons/amenities/arcticons--aeg-kitchen.svg", IsActive = true },
        new Anemities { Name = "Лифт", Description = "В здании есть лифт", IconUrl = "/icons/amenities/guidance--elevator.svg", IsActive = true },
        new Anemities { Name = "Фен", Description = "Фен для сушки волос", IconUrl = "/icons/amenities/ph--hair-dryer-thin.svg", IsActive = true },
        new Anemities { Name = "Портативная система кондиционирования", Description = "Кондиционер или система охлаждения воздуха", IconUrl = "/icons/amenities/carbon--temperature-frigid.svg", IsActive = true },
        new Anemities { Name = "Телевизор", Description = "Телевизор с плоским экраном", IconUrl = "/icons/amenities/arcticons--aerial-tv.svg", IsActive = true },
        new Anemities { Name = "Отдельное место для сна", Description = "Изолированная спальная зона", IconUrl = "/icons/amenities/cuida--bed-outiline.svg", IsActive = true },
        new Anemities { Name = "Дополнительные кровати", Description = "Возможность предоставления дополнительных спальных мест", IconUrl = "/icons/amenities/carbon--hospital-bed.svg", IsActive = true },
        new Anemities { Name = "Стиральная машина", Description = "Стиральная машина для гостей", IconUrl = "/icons/amenities/icon-park-twotone--washing-machine.svg", IsActive = true },
        new Anemities { Name = "Утюг", Description = "Утюг и гладильная доска", IconUrl = "/icons/amenities/tabler--ironing-2.svg", IsActive = true },
        new Anemities { Name = "Бассейн", Description = "Открытый бассейн", IconUrl = "/icons/amenities/cil--pool.svg", IsActive = true }
         };

        public static async Task SeedAsync(AppDbContext context)
        {
            if (await context.Anemities.AnyAsync())
            {
                return;
            }
            
            foreach (var amenity in amenities)
            {
                try
                {
                    if (amenity.Id == Guid.Empty)
                    {
                        amenity.Id = Guid.CreateVersion7();
                    }

                    await SeedAmenityAsync(context, amenity);
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Ошибка при добавлении удобства '{amenity.Name}': {ex.Message}");
                }
            }
        }

        private static async Task<bool> SeedAmenityAsync(AppDbContext context, Anemities amenity)
        {
            if (await context.Anemities.AnyAsync(a => a.Name == amenity.Name))
            {
                throw new ArgumentException("Удобство уже существует", nameof(amenity.Name));
            }
            else { 
            await context.Anemities.AddAsync(amenity);
            await context.SaveChangesAsync();
            return true;
            }
        }
    }
}
