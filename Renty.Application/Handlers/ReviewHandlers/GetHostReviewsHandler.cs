using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetReviews;
using Renty.Application.Queries;
using Renty.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Renty.Application.Handlers.ReviewHandlers
{
    public class GetHostReviewsHandler : IRequestHandler<GetHostReviewsQuery, OperationResult<GetReviewsResponse>>
    {
        private readonly AppDbContext _context;

        public GetHostReviewsHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationResult<GetReviewsResponse>> Handle(GetHostReviewsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Reviews
                .AsNoTracking()
                .Where(r => r.Property.HostId == request.HostId);

            var totalCount = await query.CountAsync(cancellationToken);

            var reviews = await query
                .Include(r => r.User)
                .Include(r => r.Property)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

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
                CreatedAt = r.CreatedAt,
                PropertySlug = r.Property?.Slug,
                PropertyName = r.Property?.Name
            }).ToList();

            var response = new GetReviewsResponse
            {
                Reviews = reviewDtos,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };

            return OperationResult<GetReviewsResponse>.Success(response);
        }
    }
}
