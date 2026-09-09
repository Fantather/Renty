using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Renty.Application.Common;
using Renty.Application.DTOs.GetProperties;
using Renty.Application.DTOs.GetProperty;
using Renty.Application.Queries;
using Renty.Domain.Interfaces;
using Renty.Infrastructure.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Renty.Application.Handlers.PropertyHandlers
{
    public class GetPropertyDetailsHandler : IRequestHandler<GetPropertyDetailsQuery, OperationResult<GetPropertyDetailsResponse>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetPropertyDetailsHandler(IPropertyRepository propertyRepository,AppDbContext context, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<OperationResult<GetPropertyDetailsResponse>> Handle(GetPropertyDetailsQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.PropertySlug))
                return OperationResult<GetPropertyDetailsResponse>.Fail("PropertySlug is null or empty");

            // Получаем данные о недвижимости из репозитория
            var property = await _propertyRepository.GetPropertyWithDetailsAsync(request.PropertySlug, cancellationToken);

            if (property == null)
                return OperationResult<GetPropertyDetailsResponse>.Fail("Property not found");

            //маппинг
            var propertyDto = _mapper.Map<GetPropertyDetailsResponse>(property);

            propertyDto.RatingBreakdown = await _context.Properties
             .Where(p => p.Slug == request.PropertySlug)
             .Select(p => new RatingBreakdownDto
             {
                 Cleanliness = p.Reviews.Any() ? Math.Round(p.Reviews.Average(r => r.CleanlinessRating).GetValueOrDefault(), 1) : 0,
                 Accuracy = p.Reviews.Any() ? Math.Round(p.Reviews.Average(r => r.AccuracyRating).GetValueOrDefault(), 1) : 0,
                 CheckIn = p.Reviews.Any() ? Math.Round(p.Reviews.Average(r => r.CheckInRating).GetValueOrDefault(), 1) : 0,
                 Communication = p.Reviews.Any() ? Math.Round(p.Reviews.Average(r => r.CommunicationRating).GetValueOrDefault(), 1) : 0,
                 Location = p.Reviews.Any() ? Math.Round(p.Reviews.Average(r => r.LocationRating).GetValueOrDefault(), 1) : 0,
                 Value = p.Reviews.Any() ? Math.Round(p.Reviews.Average(r => r.ValueRating).GetValueOrDefault(), 1) : 0
             })
             .FirstOrDefaultAsync(cancellationToken) ?? new RatingBreakdownDto();

            //пропущенное поле
            propertyDto.BookedRanges = property.Bookings
                .Where(b => b.CheckOutDate > DateTime.UtcNow)
                .Select(b => new BookedRangeDto
                {
                    From = b.CheckInDate,
                    To = b.CheckOutDate
                })
                .ToList();


            // isFavorite
            propertyDto.IsFavorite = request.UserId.HasValue && property.Favorites.Any(f => f.UserId == request.UserId);

            return OperationResult<GetPropertyDetailsResponse>.Success(propertyDto);
        }
    }
}