using MediatR;
using Renty.Application.Commands.EditCommands;
using Renty.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers
{
    public class UploadAvatarHandler : IRequestHandler<UploadAvatarCommand, OperationResult<string>>
    {
        public async Task<OperationResult<string>> Handle(UploadAvatarCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var fileExtension = Path.GetExtension(request.FileName);
                var newFileName = $"{Guid.NewGuid()}{fileExtension}";

                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users", "avatars");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                var filePath = Path.Combine(folderPath, newFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.FileStream.CopyToAsync(fileStream, cancellationToken);
                }

                var avatarUrl = $"/users/avatars/{newFileName}";

                return OperationResult<string>.Success(avatarUrl);
            }
            catch (Exception ex)
            {
                return OperationResult<string>.Fail($"Ошибка при сохранении файла: {ex.Message}");
            }
        }
    }
}
