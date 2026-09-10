using Renty.Application.Services;
using Renty.Domain.Interfaces;
using Renty.Infrastructure.Repository;
using Renty.Infrastructure.Services;
using Renty.Infrastructure.Services.CountryStateCityAPI;

namespace Renty.Web.DI
{
    static public class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
        {
            // Биндинг значениями из конфигурации appsettings.json/secret.json с свойствами класса
            services.Configure<CountryStateCityApiOptions>(
                config.GetSection(CountryStateCityApiOptions.SectionName));

            services.AddSingleton<CountryStateCityAPI>();

            services.Configure<EmailConfiguration>(
                config.GetSection
                ("EmailConfiguration"));

            services.AddSingleton<IEmailSender, EmailSender>();

            return services;
        }
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration? config = null)
        {
            services.AddScoped<IPropertiesCategoryRepository, PropertiesCategoryRepository>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IAmenityRepository, AmenitiesRepository>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<ILocationResolverService, LocationResolverService>();

            return services;
        }
    }
}
