using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Renty.Application.Handlers.PropertyHandlers;
using Renty.Application.Mappers.Properties;
using Renty.Application.Services;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.User;
using Renty.Infrastructure.Data;
using Renty.Infrastructure.Seeders;
using Renty.Infrastructure.Seeders.location;
using Renty.Infrastructure.Services.PlacesAPI;
using Renty.Web.DI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Add("/Views/Shared/PropertyForms/{0}.cshtml");
});

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(GetPropertiesHandler).Assembly);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    options.CallbackPath = "/signin-google";
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.UseNetTopologySuite())
    .EnableSensitiveDataLogging());

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // При попытке войти без подтверждения email
    // будет возвращать result.IsNotAllowed = true
    options.SignIn.RequireConfirmedEmail = true;
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    // Время жизни токена подтверждения email
    options.TokenLifespan = TimeSpan.FromHours(24);
});
// Регистрация репозиториев в DI
builder.Services.AddInfrastructure();

// Регистрация сервисов в DI
builder.Services.AddServices(builder.Configuration);

// Регистрация AutoMapper 
builder.Services.AddAutoMapper(cfg =>
{
    cfg.LicenseKey = builder.Configuration["MapperLisence:Key"];
},
typeof(Renty.Application.Mappers.Properties.PropertyProfile),
typeof(Renty.Web.Mappers.Shared.SharedProfile));

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

app.UseAuthentication();
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

        await LanguagesSeeder.SeedAsync(context);
        // Локации
        var basePath = Path.Combine(AppContext.BaseDirectory, "Seeders", "location", "SourceFiles");
        await LocationSeeder.SeedLocationsAsync(context, basePath);
        // Пользователи, админ, два одессита и киевлянин
        await IdentitySeeder.SeedAdminAsync(userManager, roleManager, context);
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
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ошибка при сидировании базы данных.");
    }
}
app.Run();
