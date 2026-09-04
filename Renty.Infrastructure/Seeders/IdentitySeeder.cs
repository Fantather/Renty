using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Renty.Domain.Models.User;
using Renty.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace Renty.Infrastructure.Seeders
{
    public static class IdentitySeeder
    {
        public static async Task SeedAdminAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager)
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
                var adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FirstName = "Renty",
                    LastName = "Boss",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "zfY8d4bKWjY");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, admin);
                }
            }
        }

        public static async Task SeedTestUsersAsync(
            UserManager<ApplicationUser> userManager,
            AppDbContext context)
        {
            var ukraine = await context.Countries.FirstOrDefaultAsync(c => c.Name == "Украина");
            var kyiv = await context.Cities.FirstOrDefaultAsync(c => c.Name == "Киев");
            var odesa = await context.Cities.FirstOrDefaultAsync(c => c.Name == "Одесса");

            // Проверяем, что локации существуют, иначе привязать пользователей не получится
            if (ukraine == null || kyiv == null || odesa == null)
            {
                Console.WriteLine("Ошибка сидирования пользователей: локации не найдены. Убедитесь, что CountrySeeder был запущен первым.");
                return;
            }

            var users = new ApplicationUser[]
            {
                new ApplicationUser
                {
                    UserName = "izya-troff",
                    Email = "laplas@renty.com",
                    FirstName = "Изя",
                    LastName = "Трофимивич",
                    EmailConfirmed = true,
                    HomeCountryId = ukraine.Id,
                    HomeCityId = odesa.Id
                },
                new ApplicationUser
                {
                    UserName = "gretta-user",
                    Email = "gretta@renty.com",
                    FirstName = "Грета",
                    LastName = "Саацбаум",
                    EmailConfirmed = true,
                    HomeCountryId = ukraine.Id,
                    HomeCityId = odesa.Id
                },
                new ApplicationUser
                {
                    UserName = "psyduck-user",
                    Email = "psyduck@renty.com",
                    FirstName = "Псайдак",
                    LastName = "Даксон",
                    EmailConfirmed = true,
                    HomeCountryId = ukraine.Id,
                    HomeCityId = kyiv.Id
                }
            };

            foreach (var user in users)
            {
                try
                {
                    var password = "zfY8d4bKWjY!";
                    await SeedUserAsync(userManager, user, password);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при создании пользователя {user.UserName}: {ex.Message}");
                }
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