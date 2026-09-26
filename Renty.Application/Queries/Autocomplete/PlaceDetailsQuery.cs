using MediatR;
using Renty.Application.Common;
using Renty.Domain.ServiceModels.Places;

namespace Renty.Application.Queries.Autocomplete
{
    /// <summary>
    /// Запрос на получение адреса по частям для места, выбранного в автозаполнении
    /// </summary>
    /// <param name="PlaceId">Идентификатор места из подсказки автозаполнения</param>
    public record PlaceDetailsQuery(string PlaceId) : IRequest<OperationResult<PlaceAddressDto>>;
}
