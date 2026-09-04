using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Renty.Domain.Models.User;
using Renty.Infrastructure.Data;
using Renty.Infrastructure.Seeders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Временно отключено — падает при старте, пустая настройка не находит сборку с обработчиками
// builder.Services.AddMediatR(cfg => { });

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.UseNetTopologySuite()));

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        // Локации(города, так как мне нужно протестировать)
        await CountrySeeder.SeedAsync(context);

        // Пользователи, админ, два одессита и киевлянин
        await IdentitySeeder.SeedAdminAsync(userManager, roleManager);
        await IdentitySeeder.SeedTestUsersAsync(userManager, context);

        // Справочники
        await PropertyCategorySeeder.SeedAsync(context);
        await RoomTypesSeeder.SeedAsync(context);
        await TagSeeder.SeedAsync(context);
        await AmenitiesSeeder.SeedAsync(context);

        // Квартиры
        await PropertySeeder.SeedAsync(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
app.Run();
