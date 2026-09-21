using MediatR;
using Microsoft.EntityFrameworkCore;
using Renty.Application.Common;
using Renty.Application.DTOs.GetUser;
using Renty.Application.Extensions;
using Renty.Application.Queries;
using Renty.Infrastructure.Data;

namespace Renty.Application.Handlers.UserHandlers
{
    public class GetEditUserProfileHandler : IRequestHandler<GetEditUserProfileQuery, OperationResult<GetEditUserProfileResponse>>
    {
        private readonly AppDbContext _context;

        public GetEditUserProfileHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<GetEditUserProfileResponse>> Handle(GetEditUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(u => u.Languages)
                .Include(u => u.Facts)
                .Include(u => u.HomeCity)
                .Include(u => u.HomeCountry)
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
                return OperationResult<GetEditUserProfileResponse>.Fail("User not found");

            var input = new EditUserProfileInputDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                AvatarUrl = user.AvatarUrl,
                HomeCityId = user.HomeCityId,
                HomeCityDisplay = user.HomeCity != null
                    ? (string.IsNullOrWhiteSpace(user.HomeCity.NameRu) ? user.HomeCity.Name : user.HomeCity.NameRu) +
                      (user.HomeCountry != null ? ", " + (string.IsNullOrWhiteSpace(user.HomeCountry.NameRu) ? user.HomeCountry.Name : user.HomeCountry.NameRu) : string.Empty)
                    : string.Empty,
                LanguageIds = user.Languages?.Select(l => l.Id).ToList() ?? new List<Guid>(),
                Info = user.Info,
                Facts = user.Facts?.Select(f => new UserFactInputDto
                {
                    Type = f.Type,
                    Value = f.Value,
                    IconName = f.Type.GetMeta().IconName
                }).ToList() ?? new List<UserFactInputDto>()
            };

            var availableLanguages = await _context.Languages
                .AsNoTracking()
                .Select(l => new LanguageOptionDto { Id = l.Id, Name = l.Name })
                .ToListAsync(cancellationToken);

            var response = new GetEditUserProfileResponse
            {
                Input = input,
                AvailableLanguages = availableLanguages
            };

            return OperationResult<GetEditUserProfileResponse>.Success(response);
        }
    }
}
