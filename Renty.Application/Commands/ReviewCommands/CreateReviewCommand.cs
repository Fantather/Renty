using MediatR;
using Renty.Application.Common;

namespace Renty.Application.Commands.ReviewCommands
{
    /// <summary>
    /// Команда для создания нового отзыва к квартире
    /// </summary>
    public record CreateReviewCommand(
        Guid PropertyId,
        Guid UserId,
        decimal Rating,
        int CleanlinessRating,
        int CommunicationRating,
        int AccuracyRating,
        int LocationRating,
        string Comment = ""
    ) : IRequest<OperationResult<Unit>>;
}
