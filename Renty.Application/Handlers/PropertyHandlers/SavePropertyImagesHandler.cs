using MediatR;
using Renty.Application.Commands.PropertyCommands;
using Renty.Application.Common;
using Renty.Application.DTOs.CreateProperty;
using Renty.Application.Helpers;
using Renty.Domain.Enums;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.PropertyHandlers
{
    public class SavePropertyImagesHandler : IRequestHandler<SavePropertyImagesCommand, OperationResult<PropertyImageResponse>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPropertyImageRepository _imageRepository;
        private readonly OwnedPropertyService _ownedPropertyService;

        public SavePropertyImagesHandler(IPropertyRepository propertyRepository, IPropertyImageRepository imageRepository, OwnedPropertyService ownedPropertyService)
        {
            _propertyRepository = propertyRepository;
            _imageRepository = imageRepository;
            _ownedPropertyService = ownedPropertyService;
        }
        public async Task<OperationResult<PropertyImageResponse>> Handle(SavePropertyImagesCommand request, CancellationToken cancellationToken)
        {
            var maxFileSize = 50 * 1024 * 1024; // 50 MB

            var result = await _ownedPropertyService.GetOwnedPropertyAsync(request.PropertyId, request.CurrentUserId, cancellationToken);

            if (!result.IsSuccess)
                return OperationResult<PropertyImageResponse>.Fail(result.Errors.ToArray());

            var property = result.Data!;

            var keepIds = request.OrderedImages
                .Where(o => o.Type == OrderedImageType.Existing)
                .Select(o => o.Id)
                .ToHashSet();

            var imagesToDelete = property.PropertyImages
                .Where(pi => !keepIds.Contains(pi.Id))
                .ToList();

            foreach(var image in imagesToDelete)
            {
                await _imageRepository.DeleteAsync(image,cancellationToken);

                var fullPath = Path.Combine(request.WebRootPath, image.ImageUrl.TrimStart('/'));
                if (File.Exists(fullPath))
                    File.Delete(fullPath);
            }

            // Загрузка новых файлов с сохранением по fileIndex
            // для того чтоб сопостаить с позицией из OrderedImages
            var newImagesByIndex = new Dictionary<int, PropertyImage>();

            var uploadFolder = Path.Combine(request.WebRootPath, "upload", "images", "properties");

            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);


            foreach(var file in request.Files)
            {
                var validationError = ImageFileValidator.ValidateFile(file, maxFileSize);

                if (!validationError.IsSuccess)
                    return OperationResult<PropertyImageResponse>.Fail(validationError.Errors.ToArray());
            }


            for (int i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];

                var fileName = Guid.CreateVersion7() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream, cancellationToken);
                }

                var imageUrl = $"/upload/images/properties/{fileName}";

                newImagesByIndex[i] = new PropertyImage
                {
                    PropertyId = property.Id,
                    ImageUrl = imageUrl,
                };
                
            }

            await _imageRepository.AddRangeAsync(newImagesByIndex.Values, cancellationToken);

            var existingById = property.PropertyImages.ToDictionary(pi => pi.Id);


            for(int position = 0; position < request.OrderedImages.Count; position++)
            {
                var orderRef = request.OrderedImages[position];

                PropertyImage image;

                if (orderRef.Type == OrderedImageType.Existing)
                {
                    if (!existingById.TryGetValue(orderRef.Id!.Value, out var value))
                        return OperationResult<PropertyImageResponse>.Fail($"Image {orderRef.Id} not found");
                    image = value;
                }
                else
                {
                    if (!newImagesByIndex.TryGetValue(orderRef.FileIndex!.Value, out var value))
                        return OperationResult<PropertyImageResponse>.Fail($"Invalid index of file {orderRef.FileIndex}");
                    image = value;
                }

                image.DisplayOrder = position;
                image.IsPrimary = position == 0;
            }

            await _imageRepository.SaveChangesAsync();

            var allImages = property.PropertyImages.ToList();

            var response = new PropertyImageResponse 
            {
                PropertyId = property.Id,
                ImageIds = allImages.Select(i => i.Id).ToList(),
                ImageUrls = allImages.Select(i => i.ImageUrl).ToList()
            };

            return OperationResult<PropertyImageResponse>.Success(response);
        }
    }
}
