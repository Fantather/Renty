using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Renty.Application.Common;
using Renty.Application.DTOs.GetProperties;
using Renty.Application.Extensions;
using Renty.Application.Queries.Property;
using Renty.Domain.Models.LookupsTables;
using Renty.Infrastructure.Data;


namespace Renty.Application.Handlers.PropertyHandlers
{
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
            var page = 1;
            var pageSize = 5; 

            try
            {
                // Валидация пагинации
                if (request.Page < page)
                    return OperationResult<GetPropertiesByMapResponse>.Fail($"The page cannot be less than {page}");

                if (request.PageSize < pageSize)
                    return OperationResult<GetPropertiesByMapResponse>.Fail($"The page size cannot be less than {pageSize}");

                var durationString = PropertyQueryExtensions.GetDurationString(request.CheckInDate, request.CheckOutDate);

                var query = _context.Properties
                    .AsNoTracking()
                    .Where(p => p.Status == PropertyStatusEnum.Active);

                // Фильтрация по координатам
                query = query.Where(p => p.Address != null && p.Address.Location != null &&
                                         p.Address.Location.Y <= request.North &&
                                         p.Address.Location.Y >= request.South &&
                                         p.Address.Location.X <= request.East &&
                                         p.Address.Location.X >= request.West);

                // Применение общих фильтров
                query = query.ApplyFilters(
                    request.GuestCount,
                    request.CheckInDate,
                    request.CheckOutDate,
                    request.CategorySlug,
                    request.AmenityIds,
                    request.PetsAllowed);

                var totalCount = await query.CountAsync(cancellationToken);

                // Сортировка по умолчанию
                query = query.ApplySort(request.SortBy);

                var properties = await query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ProjectTo<PropertyListItem>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                var favoriteSlugs = new HashSet<string>();
                if (request.UserId.HasValue && properties.Any())
                {
                    var propertySlugs = properties.Select(p => p.Slug).ToList();
                    var favoritesFromDb = await _context.Favorites
                        .Where(f => f.UserId == request.UserId && propertySlugs.Contains(f.Property.Slug))
                        .Select(f => f.Property.Slug)
                        .ToListAsync(cancellationToken);

                    favoriteSlugs = new HashSet<string>(favoritesFromDb);
                }

                foreach (var prop in properties)
                {
                    prop.Duration = durationString; 
                    if (request.UserId.HasValue)
                    {
                        prop.IsFavorite = favoriteSlugs.Contains(prop.Slug);
                    }
                }

                var response = new GetPropertiesByMapResponse
                {
                    Properties = properties,
                    TotalCount = totalCount,
                    Page = request.Page,
                    PageSize = request.PageSize
                };

                return OperationResult<GetPropertiesByMapResponse>.Success(response);
            }
            catch (Exception ex)
            {
                return OperationResult<GetPropertiesByMapResponse>.Fail(ex.Message);
            }
        }
    }
}
