using Renty.Application.Common;
using Renty.Application.DTOs.GetProperties;
using MediatR;


namespace Renty.Application.Queries.Property
{
    public record GetPropertiesByMapQuery(
    double North, double South, double East, double West,
    string? CategorySlug = null, DateTime? CheckInDate = null, DateTime? CheckOutDate = null,
    int? GuestCount = null, string? Destination = null, Guid? UserId = null, int Page = 1,         
        int PageSize = 20
) : IRequest<OperationResult<GetPropertiesByMapResponse>>;

}
