using MediatR;
using Renty.Application.Commands.CategoryCommands;
using Renty.Application.Common;
using Renty.Application.Helpers;
using Renty.Domain.Interfaces;
using Renty.Domain.Models;
using Renty.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.CategoryHandlers
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, OperationResult<Unit>>
    {
        private readonly IPropertiesCategoryRepository _categoryRepository;
        public CreateCategoryHandler(IPropertiesCategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<OperationResult<Unit>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var maxFileSize = 10 * 1024 * 1024; // 10 MB

            if (string.IsNullOrEmpty(request.Name))
                return OperationResult<Unit>.Fail("Name is null or empty");

            var id = Guid.CreateVersion7();

            var category = new PropertiesCategory
            {
                Id = id,
                Slug = SlugGenerator.GenerateSlug(request.Name, id),
                Name = request.Name,
                Description = request.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            if (request.ImageFile != null)
            {
                var validationFile = ImageFileValidator.ValidateFile(request.ImageFile, maxFileSize);

                if (!validationFile.IsSuccess)
                    return OperationResult<Unit>.Fail(validationFile.Errors.ToArray());

                var uploadFolder = Path.Combine(request.WebRootPath, "upload", "images", "categories");

                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                var fileName = Guid.CreateVersion7() + Path.GetExtension(request.ImageFile.FileName);
                var filePath = Path.Combine(uploadFolder, fileName);

                using(var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream, cancellationToken);
                }

                var imageUrl = $"/upload/images/categories/{fileName}";

                category.ImageUrl = imageUrl;
            }
            else
            {
                category.ImageUrl = "/upload/images/categories/default.png";
            }

            await _categoryRepository.AddAsync(category, cancellationToken);

            return OperationResult<Unit>.Success(new Unit());
        }
    }
}
