using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetReviews;
using Renty.Application.Queries;
using Renty.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers
{
    public class GetPropertyReviewsHandler : IRequestHandler<GetPropertyReviewsQuery, OperationResult<GetReviewsResponse>>
    {
        private readonly IReviewRepository _reviewRepository;
        public GetPropertyReviewsHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        public async Task<OperationResult<GetReviewsResponse>> Handle(GetPropertyReviewsQuery request, CancellationToken cancellationToken)
        {

            var reviews = await _reviewRepository.GetReviewsByPropertyIdAsync(request.PropertyId, cancellationToken);

            // Черновой вариант маппинга
            if (reviews.Any())
            {
                var reviewDto = reviews.Select(r => new ReviewDto
                    {
                        Id = r.Id,
                        AccuracyRating = r.AccuracyRating,
                        CleanlinessRating = r.CleanlinessRating,
                        CommunicationRating = r.CommunicationRating,
                        LocationRating = r.LocationRating,
                        Rating = r.Rating,
                        Content = r.Comment,
                        Author = new AuthorDto { FullName = $"{r.User.FirstName} {r.User.LastName}", AvatarUrl = r.User.AvatarUrl },
                        HostResponse = r.HostResponse == null
                                        ? new HostResponseDto
                                        {
                                            Host = new AuthorDto
                                            {
                                                FullName = $"{r.Property.Host.FirstName} {r.Property.Host.LastName}",
                                                AvatarUrl = r.Property.Host.AvatarUrl
                                            },
                                            Content = r.HostResponse!,
                                            CreatedAt = r.HostResponseDate,
                                        }
                                        : null,
                        CreatedAt = r.CreatedAt,
                        UpdatedAt = r.UpdatedAt
                    }
                ).ToList();

                return OperationResult<GetReviewsResponse>.Success(new GetReviewsResponse { Reviews = reviewDto });
            }

            return OperationResult<GetReviewsResponse>.Fail("Reviews by property id not found");
        }
    }
}
