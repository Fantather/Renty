using Renty.Domain.Models.Properties;

namespace Renty.Application.Extensions
{
    public static class SwitchExtension
    {
        // Метод расширения для конструкции switch (сортировка)
        public static IQueryable<Property> ApplySort(this IQueryable<Property> query, string? sortBy)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return query.OrderByDescending(p => p.CreatedAt);
            }
            return sortBy switch
            {
                "RATING_ASC" => query.OrderBy(p => p.AverageRating),
                "RATING_DESC" => query.OrderByDescending(p => p.AverageRating),
                "PRICEPRENIGHT_ASC" => query.OrderBy(p => p.PricePerNight),
                "PRICEPRENIGHT_DESC" => query.OrderByDescending(p => p.PricePerNight),
                "CREATED_AT_ASC" => query.OrderBy(p => p.CreatedAt),
                _ => query.OrderByDescending(p => p.CreatedAt) 
            };
        }
    }
}
