using MediatR;
using Renty.Application.Commands.ReviewCommands;
using Renty.Application.Common;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.User;

namespace Renty.Application.Handlers
{
    /// <summary>
    /// Обработчик команды <see cref="CreateReviewCommand"/>.
    /// Создает новый отзыв к квартире и автоматически пересчитывает ее средний рейтинг.
    /// </summary>
    public class CreateReviewHandler : IRequestHandler<CreateReviewCommand, OperationResult<Unit>>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IPropertyRepository _propertyRepository;

        public CreateReviewHandler(IReviewRepository reviewRepository, IPropertyRepository propertyRepository)
        {
            _reviewRepository = reviewRepository;
            _propertyRepository = propertyRepository;
        }

        /// <summary>
        /// Обрабатывает команду на добавление отзыва.
        /// </summary>
        /// <param name="request">Объект команды с данными нового отзыва.</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
        /// <returns>Результат выполнения операции (успех или сообщение об ошибке).</returns>
        public async Task<OperationResult<Unit>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //Проверяем, существует ли квартира
                var property = await _propertyRepository.GetByIdAsync(request.PropertyId, cancellationToken);

                if (property == null)
                {
                    return OperationResult<Unit>.Fail("Квартира не найдена");
                }

                //Создаем  отзыв
                var review = new Review
                {
                    Id = Guid.CreateVersion7(),
                    PropertyId = request.PropertyId,
                    UserId = request.UserId,
                    Rating = request.Rating,
                    CleanlinessRating = request.CleanlinessRating,
                    CommunicationRating = request.CommunicationRating,
                    AccuracyRating = request.AccuracyRating,
                    LocationRating = request.LocationRating,
                    Comment = request.Comment,
                    CreatedAt = DateTime.UtcNow
                };


                await _reviewRepository.AddAsync(review, cancellationToken);

                //пересчитывание среднего рейтинга квартиры
                var currentTotalScore = property.AverageRating * property.ReviewsCount;
                var newTotalScore = currentTotalScore + request.Rating;

                property.ReviewsCount += 1;
                property.AverageRating = Math.Round(newTotalScore / property.ReviewsCount, 2);

                //Обновляем БД
                await _propertyRepository.UpdateAsync(property, cancellationToken);

                return OperationResult<Unit>.Success(Unit.Value);
            }
            catch (Exception ex)
            {
                return OperationResult<Unit>.Fail(ex.Message);
            }
        }
    }
}