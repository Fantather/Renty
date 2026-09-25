using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Renty.Application.Common;
using Renty.Application.DTOs.GetProperties;
using Renty.Application.Extensions;
using Renty.Application.Queries;
using Renty.Domain.Models.LookupsTables;
using Renty.Infrastructure.Data;
using Renty.Infrastructure.Helpers;

namespace Renty.Application.Handlers.PropertyHandlers
{
    public class GetPropertiesHandler : IRequestHandler<GetPropertiesQuery, OperationResult<GetPropertiesResponse>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetPropertiesHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OperationResult<GetPropertiesResponse>> Handle(GetPropertiesQuery request, CancellationToken cancellationToken)
        {
            var page = 1;
            var pageSize = 5;

            try
            {
                if (request.Page < page)
                    return OperationResult<GetPropertiesResponse>.Fail($"Страница не может быть меньше {page}");

                if (request.PageSize < pageSize)
                    return OperationResult<GetPropertiesResponse>.Fail($"Размер страницы не может быть меньше {pageSize}");

                var durationString = PropertyQueryExtensions.GetDurationString(request.CheckInDate, request.CheckOutDate);

                var query = _context.Properties
                    .Where(p => p.Status == PropertyStatusEnum.Active)
                    .AsNoTracking()
                    .AsQueryable();

                // Специфичный фильтр для этого хендлера
                if (!string.IsNullOrWhiteSpace(request.Destination))
                {
                    var destination = request.Destination.ToLower().Trim();

                    if (RuHelper.IsCyrillic(destination))
                    {
                        query = query.Where(p => p.City.NameRu != null && p.City.NameRu.ToLower().Contains(destination));
                    }
                    else
                    {
                        query = query.Where(p => p.City.Name != null && p.City.Name.ToLower().Contains(destination));
                    }
                }

                if (request.CityId.HasValue)
                    query = query.Where(p => p.CityId == request.CityId.Value);

                if (request.CategoryId.HasValue)
                    query = query.Where(p => p.CategoryId == request.CategoryId.Value);

                // общие фильры
                query = query.ApplyFilters(
                    request.GuestCount,
                    request.CheckInDate,
                    request.CheckOutDate,
                    request.CategorySlug,
                    request.AmenityIds); 

                // тотал каунт до  пагинации
                var totalCount = await query.CountAsync(cancellationToken);

                query = query.ApplySort(request.SortBy);

                var propertiesDto = await query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ProjectTo<PropertyListItem>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                var favoriteSlugs = new HashSet<string>();
                if (request.UserId.HasValue && propertiesDto.Any())
                {
                    var propertySlugs = propertiesDto.Select(p => p.Slug).ToList();
                    var favoritesFromDb = await _context.Favorites
                        .Where(f => f.UserId == request.UserId.Value && propertySlugs.Contains(f.Property.Slug))
                        .Select(f => f.Property.Slug)
                        .ToListAsync(cancellationToken);
                    favoriteSlugs = new HashSet<string>(favoritesFromDb);
                }

                foreach (var dto in propertiesDto)
                {
                    dto.Duration = durationString;
                    if (request.UserId.HasValue)
                    {
                        dto.IsFavorite = favoriteSlugs.Contains(dto.Slug);
                    }
                }


                return OperationResult<GetPropertiesResponse>.Success(new GetPropertiesResponse
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    TotalCount = totalCount, 
                    Properties = propertiesDto
                });
            }
            catch (Exception ex)
            {
                return OperationResult<GetPropertiesResponse>.Fail(ex.Message);
            }
        }
    }
}

