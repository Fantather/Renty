using AutoMapper;
using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetProperty;
using Renty.Application.Queries;
using Renty.Domain.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Renty.Application.Handlers.PropertyHandlers
{
    public class GetPropertyDetailsHandler : IRequestHandler<GetPropertyDetailsQuery, OperationResult<GetPropertyDetailsResponse>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper; 

        public GetPropertyDetailsHandler(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
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

            // isFavorite
            propertyDto.IsFavorite = request.UserId.HasValue && property.Favorites.Any(f => f.UserId == request.UserId);

            return OperationResult<GetPropertyDetailsResponse>.Success(propertyDto);
        }
    }
}