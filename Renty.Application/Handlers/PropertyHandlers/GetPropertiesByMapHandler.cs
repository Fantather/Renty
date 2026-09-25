using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Renty.Application.Common;
using Renty.Application.DTOs.GetProperties;
using Renty.Application.Queries.Property;
using Renty.Domain.Models.LookupsTables;
using Renty.Infrastructure.Data;


namespace Renty.Application.Handlers.PropertyHandlers
{
    // TODO (Ольга): чтобы карта и список на странице поиска (SearchController.Index) показывали одно и то же, не хватает того,
    // что делает GetPropertiesHandler при тех же фильтрах:
    // 1) Duration: строка «27 сент - 2 окт (5 ночей)» для каждого PropertyListItem, иначе после сдвига карты у карточек пропадает подпись с датами;
    // 2) сортировка: перед Skip/Take нет OrderBy, порядок и страницы недетерминированы; нужен SortBy (по умолчанию CreatedAt по убыванию на пример);
    // 3) бронирования: тут учитываются только Confirmed, в GetPropertiesHandler любые; и нет случаев «только дата заезда» и «только дата выезда»;
    // 4) в GetPropertiesByMapQuery объявлены Destination и CategorySlugs, но хендлер их не применяет: либо применить, либо убрать из запроса;
    // 5) валидация Page/PageSize и try/catch с OperationResult.Fail, как в GetPropertiesHandler;
    // 6) общие фильтры (даты, гости, категория, удобства) лучше вынести в один метод-расширение ApplyFilters для обоих хендлеров,
    //    иначе они снова разойдутся.
    public class GetPropertiesByMapHandler : IRequestHandler<GetPropertiesByMapQuery, OperationResult<GetPropertiesByMapResponse>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetPropertiesByMapHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OperationResult<GetPropertiesByMapResponse>> Handle(GetPropertiesByMapQuery request, CancellationToken cancellationToken)
        {
            // База
            var query = _context.Properties
                .AsNoTracking()
                .Where(p => p.Status == PropertyStatusEnum.Active);

            // Фильтрация по видимой области карты
            query = query.Where(p => p.Address != null && p.Address.Location != null &&
                                     p.Address.Location.Y <= request.North &&
                                     p.Address.Location.Y >= request.South &&
                                     p.Address.Location.X <= request.East &&
                                     p.Address.Location.X >= request.West);

            // фильтрация по категории, если указана
            if (!string.IsNullOrWhiteSpace(request.CategorySlug))
            {
                query = query.Where(p => p.Category.Slug == request.CategorySlug);
            }
            // фильтрация по списку удобств, если указанны
            if (request.AmenityIds != null && request.AmenityIds.Any())
            {
                foreach (var amenityId in request.AmenityIds)
                {
                    query = query.Where(p => p.PropertyAmenities.Any(pa => pa.AmenityId == amenityId && pa.IsActive));
                }
            }

            // Фильтрация по количеству гостей
            if (request.GuestCount.HasValue && request.GuestCount > 0)
            {
                query = query.Where(p => p.Details != null && p.Details.MaxGuests >= request.GuestCount);
            }

            // Исключение квартир если пересекающиеися подтвержденные бронирования
            if (request.CheckInDate.HasValue && request.CheckOutDate.HasValue)
            {
                query = query.Where(p => !p.Bookings.Any(b =>
                    b.Status == BookingStatusEnum.Confirmed &&
                    b.CheckInDate < request.CheckOutDate &&
                    b.CheckOutDate > request.CheckInDate));
            }

            // Общее количество квартир
            var totalCount = await query.CountAsync(cancellationToken);

            // Выборка и маппинг с пагинацией
            var properties = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<PropertyListItem>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            // IsFavorite, если пользователь авторизован
            if (request.UserId.HasValue && properties.Any())
            {
                var propertySlugs = properties.Select(p => p.Slug).ToList();

                var favoriteSlugs = await _context.Favorites
                    .Where(f => f.UserId == request.UserId && propertySlugs.Contains(f.Property.Slug))
                    .Select(f => f.Property.Slug)
                    .ToListAsync(cancellationToken);

                foreach (var prop in properties)
                {
                    prop.IsFavorite = favoriteSlugs.Contains(prop.Slug);
                }
            }

            // итог
            var response = new GetPropertiesByMapResponse
            {
                Properties = properties,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };

            return OperationResult<GetPropertiesByMapResponse>.Success(response);
        }
    }
}
