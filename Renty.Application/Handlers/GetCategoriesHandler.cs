using AutoMapper;
using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.Common;
using Renty.Application.DTOs.GetCategories;
using Renty.Application.Queries;
using Renty.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Renty.Application.Handlers
{
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, OperationResult<GetCategoriesResponse>>
    {
        private readonly IPropertiesCategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetCategoriesHandler(IPropertiesCategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
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

            if (categories.Any())
            {
                var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);
                return OperationResult<GetCategoriesResponse>.Success(new GetCategoriesResponse { Categories = categoriesDto });
            }

            return OperationResult<GetCategoriesResponse>.Fail("Categories not found");
        }
    }
}