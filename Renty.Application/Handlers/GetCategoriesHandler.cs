using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.Common;
using Renty.Application.DTOs.GetCategories;
using Renty.Application.Queries;
using Renty.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers
{
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, OperationResult<GetCategoriesResponse>>
    {
        private readonly IPropertiesCategoryRepository _categoryRepository;

        public GetCategoriesHandler(IPropertiesCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        /// <summary>
        /// Получение всех категорий
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>Список категорий</returns>
        public async Task<OperationResult<GetCategoriesResponse>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllActiveAsync(cancellationToken);

            // Черновой вариант маппинга

            if (categories.Any())
            {
                var categoriesDto = categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Slug = c.Slug,
                    Name = c.Name,
                    Description = c.Description,
                    ImageUrl = c.ImageUrl,
                }).ToList();

                return OperationResult<GetCategoriesResponse>.Success(new GetCategoriesResponse {Categories = categoriesDto});
            }

            return OperationResult<GetCategoriesResponse>.Fail("Categories not found");
        }
    }
}
