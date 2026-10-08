using AutoMapper;
using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetReviews;
using Renty.Application.Queries;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.User;

namespace Renty.Application.Handlers.ReviewHandlers
{
    public class GetPropertyReviewsHandler : IRequestHandler<GetPropertyReviewsQuery, OperationResult<GetReviewsResponse>>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public GetPropertyReviewsHandler(IReviewRepository reviewRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<GetReviewsResponse>> Handle(GetPropertyReviewsQuery request, CancellationToken cancellationToken)
        {
            // 1. Защита от невалидных параметров пагинации с фронтенда
            var page = request.Page < 1 ? 1 : request.Page;
            var pageSize = request.PageSize < 1 ? 10 : request.PageSize;

            IEnumerable<Review> reviews = new List<Review>();
            int totalCount = 0;

            // 2. Разветвление: определяем, по какому параметру искать отзывы
            if (!string.IsNullOrWhiteSpace(request.Slug))
            {
                var result = await _reviewRepository.GetReviewsByPropertySlugPaginatedAsync(request.Slug, page, pageSize, cancellationToken);
                reviews = result.Reviews;
                totalCount = result.TotalCount;
            }
            else if (request.PropertyId.HasValue && request.PropertyId.Value != Guid.Empty)
            {
                var result = await _reviewRepository.GetReviewsByPropertyIdPaginatedAsync(request.PropertyId.Value, page, pageSize, cancellationToken);
                reviews = result.Reviews;
                totalCount = result.TotalCount;
            }
            else
            {
                return OperationResult<GetReviewsResponse>.Fail("Необходимо передать либо PropertyId, либо Slug.");
            }

            var reviewDtos = reviews.Select(r => _mapper.Map<ReviewDto>(r)).ToList();

            var response = new GetReviewsResponse
            {
                Reviews = reviewDtos,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            return OperationResult<GetReviewsResponse>.Success(response);
        }
    }
}
