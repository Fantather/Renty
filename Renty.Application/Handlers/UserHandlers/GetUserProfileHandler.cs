using MediatR;
using Microsoft.EntityFrameworkCore;
using Renty.Application.Common;
using Renty.Application.DTOs.GetUser;
using Renty.Application.Extensions;
using Renty.Application.Queries;
using Renty.Infrastructure.Data;
using Renty.Application.DTOs.GetReviews;

namespace Renty.Application.Handlers.UserHandlers
{
    public class GetUserProfileHandler : IRequestHandler<GetUserProfileQuery, OperationResult<GetUserProfileResponse>>
    {
        private readonly AppDbContext _context;

        public GetUserProfileHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<GetUserProfileResponse>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Include(u => u.Languages)
                .Include(u => u.Facts)
                .Include(u => u.HomeCity)
                .Include(u => u.HomeCountry)
                .FirstOrDefaultAsync(u => u.Id == request.TargetUserId, cancellationToken);

            if (user == null)
                return OperationResult<GetUserProfileResponse>.Fail("User not found");

            var reviews = await _context.Reviews
                .AsNoTracking()
                .Include(r => r.User)
                .Include(r => r.Property)
                .Where(r => r.Property.HostId == user.Id)
                .OrderByDescending(r => r.CreatedAt)
                .Take(20)
                .ToListAsync(cancellationToken);

            // Проверка владельца через переданный CurrentUserId
            bool isOwner = request.CurrentUserId.HasValue && request.CurrentUserId.Value == user.Id;

            var months = ((DateTime.UtcNow.Year - user.CreatedAt.Year) * 12) + (DateTime.UtcNow.Month - user.CreatedAt.Month);
            var languages = user.Languages?.Select(l => l.Name).ToList() ?? new List<string>();

            string homeCity = string.Empty;
            if (user.HomeCity != null)
                homeCity = string.IsNullOrWhiteSpace(user.HomeCity.NameRu) ? user.HomeCity.Name : user.HomeCity.NameRu;

            string homeCountry = string.Empty;
            if (user.HomeCountry != null)
                homeCountry = string.IsNullOrWhiteSpace(user.HomeCountry.NameRu) ? user.HomeCountry.Name : user.HomeCountry.NameRu;

            var facts = user.Facts?.Select(f => new UserFactDto
            {
                Type = f.Type.ToString(),
                Value = f.Value,
                IconName = f.Type.GetMeta().IconName
            }).ToList() ?? new List<UserFactDto>();

            var reviewDtos = reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                Author = new AuthorDto
                {
                    FullName = (r.User?.FirstName + " " + r.User?.LastName).Trim(),
                    AvatarUrl = r.User?.AvatarUrl
                },
                Rating = r.Rating,
                Content = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList();

            var avgRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0m;

            var response = new GetUserProfileResponse
            {
                UserId = user.Id,
                IsOwner = isOwner,
                AvatarUrl = string.IsNullOrWhiteSpace(user.AvatarUrl) ? "https://placehold.co/160x160" : user.AvatarUrl,
                FullName = (user.FirstName + " " + user.LastName).Trim(),
                IsSuperHost = user.IsSuperHost,
                Rating = Math.Round(avgRating, 2),
                ReviewsCount = reviews.Count,
                MonthsOnPlatform = Math.Max(0, months),
                IsVerified = user.IsVerified,
                HomeCity = homeCity,
                HomeCountry = homeCountry,
                Languages = languages,
                Info = user.Info,
                Facts = facts,
                Reviews = reviewDtos
            };

            return OperationResult<GetUserProfileResponse>.Success(response);
        }
    }
}
