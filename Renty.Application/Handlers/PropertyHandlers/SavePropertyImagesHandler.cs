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
    public class SavePropertyImagesHandler : IRequestHandler<SavePropertyImagesCommand, OperationResult<List<OrderedImageDto>>>
    {
        private readonly IPropertyImageRepository _imageRepository;
        private readonly OwnedPropertyService _ownedPropertyService;

        public SavePropertyImagesHandler(
            IPropertyImageRepository imageRepository, 
            OwnedPropertyService ownedPropertyService)
        {
            _imageRepository = imageRepository;
            _ownedPropertyService = ownedPropertyService;
        }
        public async Task<OperationResult<List<OrderedImageDto>>> Handle(SavePropertyImagesCommand request, CancellationToken cancellationToken)
        {
            var maxFileSize = 50 * 1024 * 1024; // 50 MB

            var result = await _ownedPropertyService.GetOwnedPropertyAsync(request.PropertyId, request.CurrentUserId, cancellationToken);

            if (!result.IsSuccess)
                return OperationResult<List<OrderedImageDto>>.Fail(result.Errors.ToArray());

            var property = result.Data!;

            var keepIds = request.OrderedImages
                .Where(o => o.Type == OrderedImageType.Existing)
                .Select(o => o.Id)
                .ToHashSet();

            var imagesToDelete = property.PropertyImages
                .Where(pi => !keepIds.Contains(pi.Id))
                .ToList();

            // Загрузка новых файлов с сохранением по fileIndex
            // для того чтоб сопостаить с позицией из OrderedImages
            var newImagesByIndex = new Dictionary<int, PropertyImage>();

            var uploadFolder = Path.Combine(request.WebRootPath, "upload", "images");

            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);


            foreach(var file in request.Files)
            {
                var validationError = ImageFileValidator.ValidateFile(file, maxFileSize);

                if (!validationError.IsSuccess)
                    return OperationResult<List<OrderedImageDto>>.Fail(validationError.Errors.ToArray());
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

                var imageUrl = $"/upload/images/{fileName}";

                newImagesByIndex[i] = new PropertyImage
                {
                    PropertyId = property.Id,
                    ImageUrl = imageUrl,
                };
                
            }

            //await _imageRepository.AddRangeAsync(newImagesByIndex.Values, cancellationToken);

            foreach (var image in imagesToDelete)
            {
                await _imageRepository.DeleteAsync(image, cancellationToken);

                var fullPath = Path.Combine(request.WebRootPath, image.ImageUrl.TrimStart('/'));
                if (File.Exists(fullPath))
                    File.Delete(fullPath);
            }

            var existingById = property.PropertyImages.ToDictionary(pi => pi.Id);

            var responseImages = new List<OrderedImageDto>();

            for(int position = 0; position < request.OrderedImages.Count; position++)
            {
                var orderRef = request.OrderedImages[position];

                PropertyImage image;

                if (orderRef.Type == OrderedImageType.Existing)
                {
                    if (!existingById.TryGetValue(orderRef.Id!.Value, out var value))
                        return OperationResult<List<OrderedImageDto>>.Fail($"Image {orderRef.Id} not found");
                    image = value;
                }
                else
                {
                    if (!newImagesByIndex.TryGetValue(orderRef.FileIndex!.Value, out var value))
                        return OperationResult<List<OrderedImageDto>>.Fail($"Invalid index of file {orderRef.FileIndex}");
                    image = value;
                }

                image.DisplayOrder = position;
                image.IsPrimary = position == 0;

                responseImages.Add(new OrderedImageDto
                {
                    ImageId = image.Id,
                    ImageUrl = image.ImageUrl,
                    IsPrimary = image.IsPrimary,
                    DisplayOrder = image.DisplayOrder
                });
            }

            // Пока не отправляются с формы OrderedImageRef
            for (int position = 0; position < newImagesByIndex.Count; position++)
            {
                var orderRef = newImagesByIndex[position];

                orderRef.DisplayOrder = position;
                orderRef.IsPrimary = position == 0;

            }
            await _imageRepository.AddRangeAsync(newImagesByIndex.Values, cancellationToken);

            //await _imageRepository.SaveChangesAsync(cancellationToken);

            return OperationResult<List<OrderedImageDto>>.Success(responseImages);
        }
    }
}
