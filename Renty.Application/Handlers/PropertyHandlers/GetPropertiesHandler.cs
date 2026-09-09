using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Renty.Application.Common;
using Renty.Application.DTOs.GetProperties;
using Renty.Application.Queries;
using Renty.Domain.Models.LookupsTables;
using Renty.Infrastructure.Data;

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
                    return OperationResult<GetPropertiesResponse>.Fail($"The page cannot be less than {page}");

                if (request.PageSize < pageSize)
                    return OperationResult<GetPropertiesResponse>.Fail($"The page size cannot be less than {pageSize}");
                //подготовка строки 
                string durationString = string.Empty;
                if (request.CheckInDate.HasValue && request.CheckOutDate.HasValue)
                {
                    var checkIn = request.CheckInDate.Value;
                    var checkOut = request.CheckOutDate.Value;
                    durationString = $"{checkIn:dd MMM} - {checkOut:dd MMM} ({(checkOut - checkIn).Days} ночей)";
                }

                //без репозитория запрос идет к бд
                var query = _context.Properties
                    .Where(p => p.Status == PropertyStatusEnum.Active)
                    .AsNoTracking()
                    .AsQueryable();

                // фильтрация
                if (request.GuestCount.HasValue)
                    query = query.Where(p => p.Details.MaxGuests >= request.GuestCount);

                if (request.CheckInDate.HasValue && request.CheckOutDate.HasValue)
                    query = query.Where(p => !p.Bookings.Any(b => b.CheckOutDate > request.CheckInDate.Value && b.CheckInDate < request.CheckOutDate.Value));

                if (request.CityId.HasValue)
                    query = query.Where(p => p.CityId == request.CityId.Value);

                if (request.CategoryId.HasValue)
                    query = query.Where(p => p.CategoryId == request.CategoryId.Value);

                if (!string.IsNullOrEmpty(request.CategorySlug))
                    query = query.Where(p => p.Category.Slug == request.CategorySlug);

                // сортировка
                query = request.SortBy switch
                {
                    "RATING_ASC" => query.OrderBy(p => p.AverageRating),
                    "RATING_DESC" => query.OrderByDescending(p => p.AverageRating),
                    "PRICEPRENIGHT_ASC" => query.OrderBy(p => p.PricePerNight),
                    "PRICEPRENIGHT_DESC" => query.OrderByDescending(p => p.PricePerNight),
                    "CREATED_AT_ASC" => query.OrderBy(p => p.CreatedAt),
                    _ => query.OrderByDescending(p => p.CreatedAt)
                };

                // пагинация и проекция в DTO
                var propertiesDto = await query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ProjectTo<PropertyListItem>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                // та самая строка в дто
                foreach (var dto in propertiesDto)
                {
                    dto.Duration = durationString;
                }

                return OperationResult<GetPropertiesResponse>.Success(
                    new GetPropertiesResponse { Page = request.Page, PageSize = request.PageSize, Properties = propertiesDto });
            }
            catch (Exception ex)
            {
                return OperationResult<GetPropertiesResponse>.Fail(ex.Message);
            }
        }
    }
}