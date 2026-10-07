using MediatR;
using Renty.Application.Commands;
using Renty.Application.Common;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.User;
using Renty.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers
{
    public class ToggleFavoriteHandler : IRequestHandler<ToggleFavoriteCommand, OperationResult<bool>>
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public ToggleFavoriteHandler(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public async Task<OperationResult<bool>> Handle(ToggleFavoriteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var isNowFavorite = await _favoriteRepository.ToggleFavoriteAsync(request.UserId, request.Slug, cancellationToken);

                return OperationResult<bool>.Success(isNowFavorite);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Fail(ex.Message);
            }
        }
    }
}
