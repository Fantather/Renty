using MediatR;
using Microsoft.AspNetCore.Http;
using Renty.Application.Common;
using Renty.Application.DTOs.CreateProperty;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Commands.PropertyCommands
{
    /// <summary>
    /// Команда сохранения фото для недвижимости
    /// </summary>
    /// <param name="PropertyId">Текущая недвижимость</param>
    /// <param name="CurrentUserId">Текущий пользователь</param>
    /// <param name="WebRootPath">Путь к wwwroot</param>
    /// <param name="OrderedImages">Последовательность изображений</param>

    /// <param name="Files">Список загруженных файлов</param>
    public record SavePropertyImagesCommand(Guid PropertyId, Guid CurrentUserId, string WebRootPath, List<OrderedImageRef> OrderedImages, List<IFormFile> Files):IRequest<OperationResult<List<OrderedImageDto>>>;

}
