using MediatR;
using Microsoft.AspNetCore.Identity;
using Renty.Application.Common;
using Renty.Application.DTOs.GetProperties;
using Renty.Application.Queries;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.User;
using Renty.Domain.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers
{
    public class GetPropertiesHandler : IRequestHandler<GetPropertiesQuery, OperationResult<GetPropertiesResponse>>
    {
        private readonly IPropertyRepository _propertyRepository;

        public GetPropertiesHandler(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
            
        }
        /// <summary>
        /// Метод фильтрует, сортирует, и использует пагинацию для отображения 
        /// </summary>
        /// <param name="request">Объект для поиска недвижимости</param>
        /// <param name="cancellationToken">Отменяющий токен</param>
        /// <returns>Возвращает список недвижимости отвечающий переданным параметрам</returns>
        public async Task<OperationResult<GetPropertiesResponse>> Handle(GetPropertiesQuery request, CancellationToken cancellationToken)
        {
            try 
            {
                if (request.Page <= 1)
                    return OperationResult<GetPropertiesResponse>.Fail("The page cannot be less than 1");

                if(request.PageSize <= 5)
                    return OperationResult<GetPropertiesResponse>.Fail("The page size cannot be less than 5");

                var param = new ParametersPropertiesForCatalog
                {
                    Skip = (request.Page - 1) * request.PageSize,
                    PageSize = request.PageSize,
                    SortBy = request.SortBy,
                    CityId = request.CityId,
                    CategoryId = request.CategoryId,
                    CategorySlug = request.CategorySlug,
                    CheckInDate = request.CheckInDate,
                    CheckOutDate = request.CheckOutDate,
                    GuestCount = request.GuestCount
                };

                var properties = await _propertyRepository.GetPropertiesForCatalogAsync(param, cancellationToken);

                // Черновой вариант маппинга
                var propertiesDto = properties.Select(p =>
                    new PropertyListItem 
                    {
                        Slug = p.Slug,
                        PropertyName = p.Name,
                        AverageRating = p.AverageRating,
                        CoverImage = p.PropertyImages.FirstOrDefault(i=>i.IsPrimary)?.ImageUrl ?? p.PropertyImages.First().ImageUrl,
                        CategoryName = p.Category.Name,
                        IsFavorite = request.UserId == null ? false : p.Favorites.Any(f => f.UserId == request.UserId),
                        CityName = p.City.Name,
                        CountryName = p.Country.Name,
                        ReviewsCount = p.ReviewsCount,
                        PricePerNight = p.PricePerNight,
                        Currency = p.Currency,
                        Duration = "",
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt
                    }
                ).ToList();

                return OperationResult<GetPropertiesResponse>.Success(new GetPropertiesResponse { Page = request.Page, PageSize = request.PageSize, Properties = propertiesDto });
            }
            catch(Exception ex)
            {
                return OperationResult<GetPropertiesResponse>.Fail(ex.Message);
            }
        }
    }
}
