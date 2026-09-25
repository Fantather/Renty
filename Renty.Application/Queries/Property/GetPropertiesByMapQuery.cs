using Renty.Application.Common;
using Renty.Application.DTOs.GetProperties;
using MediatR;


namespace Renty.Application.Queries.Property
{
    [Obsolete("Заменён GetPropertiesQuery: границы карты (North/South/East/West) теперь необязательные параметры в нём")]
    public record GetPropertiesByMapQuery(
    double North, double South, double East, double West,
    string? CategorySlug = null, DateTime? CheckInDate = null, DateTime? CheckOutDate = null,
    int? GuestCount = null, string? Destination = null, Guid? UserId = null, int Page = 1,         
        int PageSize = 20, List<string>? CategorySlugs = null, List<Guid>? AmenityIds = null
) : IRequest<OperationResult<GetPropertiesByMapResponse>>;

}
