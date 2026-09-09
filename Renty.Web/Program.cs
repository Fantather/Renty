using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Renty.Application.Handlers.PropertyHandlers;
using Renty.Domain.Models.User;
using Renty.Infrastructure.Data;
using Renty.Infrastructure.Seeders;
using Renty.Web.DI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(OLDGetPropertiesHandler).Assembly);
});

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = IdentityConstants.ApplicationScheme;
//    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
//})
//.AddCookie(options =>
//{
//    options.LoginPath = "/Account/Login";
//})
//.AddGoogle(options =>
//{
//    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
//    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
//});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        x => x.UseNetTopologySuite()));

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

// Регистрация AutoMapper и добавление профилей из сборки Renty.Application
builder.Services.AddAutoMapper(tcp => { }, typeof(PropertyProfile));

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

//app.UseAuthentication();
//app.UseAuthorization();

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
