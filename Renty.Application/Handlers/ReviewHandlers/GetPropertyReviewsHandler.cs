using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetReviews;
using Renty.Application.Queries;
using Renty.Domain.Interfaces;
using AutoMapper;

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

            var reviews = await _reviewRepository.GetReviewsByPropertyIdAsync(request.PropertyId, cancellationToken);

            if (reviews.Any())
            {
                var reviewDto = reviews.Select(r => _mapper.Map<ReviewDto>(r)).ToList();

                return OperationResult<GetReviewsResponse>.Success(new GetReviewsResponse { Reviews = reviewDto });
            }

            return OperationResult<GetReviewsResponse>.Fail("Reviews by property id not found");
        }
    }
}

