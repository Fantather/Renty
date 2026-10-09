using AutoMapper;
using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetProperties;
using Renty.Application.Queries;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.LookupsTables;
using Renty.Infrastructure.Data;


namespace Renty.Application.Handlers.UserHandlers
{
    public class GetUserFavoritesHandler : IRequestHandler<GetUserFavoritesQuery, OperationResult<List<PropertyListItem>>>
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IMapper _mapper;

        public GetUserFavoritesHandler(IFavoriteRepository favoriteRepository, IMapper mapper)
        {
            _favoriteRepository = favoriteRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<List<PropertyListItem>>> Handle(GetUserFavoritesQuery request, CancellationToken cancellationToken)
        {
            var favorites = await _favoriteRepository.GetUserFavoritesAsync(request.UserId, cancellationToken);

            var properties = favorites.Select(f => f.Property).ToList();

            var items = _mapper.Map<List<PropertyListItem>>(properties);

            foreach (var item in items)
            {
                item.IsFavorite = true;
            }

            return OperationResult<List<PropertyListItem>>.Success(items);
        }
    }
}
