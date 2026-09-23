using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Renty.Application.Common;
using Renty.Application.DTOs.GetProperties;
using Renty.Application.Queries;
using Renty.Application.Queries.Property;
using Renty.Domain.Models.LookupsTables;
using Renty.Infrastructure.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            // база
            var query = _context.Properties
                .AsNoTracking()
                .Where(p => p.Status == PropertyStatusEnum.Active);

            // фильтрация карте
            query = query.Where(p => p.Address != null && p.Address.Location != null &&
                                     p.Address.Location.Y <= request.North &&
                                     p.Address.Location.Y >= request.South &&
                                     p.Address.Location.X <= request.East &&
                                     p.Address.Location.X >= request.West);

            // применение пользовательских фильтров поиска
            if (!string.IsNullOrWhiteSpace(request.CategorySlug))
            {
                query = query.Where(p => p.Category.Slug == request.CategorySlug);
            }

            if (!string.IsNullOrWhiteSpace(request.Destination))
            {
                var destLower = request.Destination.ToLower();
                query = query.Where(p => p.City.Name.ToLower().Contains(destLower) ||
                                         (p.City.NameRu != null && p.City.NameRu.ToLower().Contains(destLower)));
            }

            // Фильтрация по количеству гостей ч
            if (request.GuestCount.HasValue && request.GuestCount > 0)
            {
                query = query.Where(p => p.Details != null && p.Details.MaxGuests >= request.GuestCount);
            }

            // убрать если есть пересек брони
            if (request.CheckInDate.HasValue && request.CheckOutDate.HasValue)
            {
                query = query.Where(p => !p.Bookings.Any(b =>
                    b.Status == BookingStatusEnum.Confirmed && 
                    b.CheckInDate < request.CheckOutDate &&
                    b.CheckOutDate > request.CheckInDate));
            }

            // общее колличество квартир
            var totalCount = await query.CountAsync(cancellationToken);

            // выборка и мапинг
            var properties = await query
                .Skip((request.Page - 1) * request.Page)
                .Take(request.PageSize)
                .ProjectTo<PropertyListItem>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            // любимые, если есть айди
            if (request.UserId.HasValue && properties.Any())
            {
                var propertySlugs = properties.Select(p => p.Slug).ToList();

                // слаг избранных
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
