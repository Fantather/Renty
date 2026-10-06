using Microsoft.AspNetCore.Hosting;
using Renty.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Helpers
{
    public class AvatarDownloadService : IAvatarDownloadService
    {
        private readonly HttpClient _httpClient;
        private readonly string _webRootPath;
        public AvatarDownloadService(
            HttpClient httpClient,
            IWebHostEnvironment env
            )
        {
            _httpClient = httpClient;
            _webRootPath = env.WebRootPath;
        }
        public async Task<string?> DownloadAndSaveAsync(string sourceUrl, Guid userId, CancellationToken ct = default)
        {
            try
            {
                var response = await _httpClient.GetAsync(sourceUrl, ct);
                if (!response.IsSuccessStatusCode)
                    return null;

                var contentType = response.Content.Headers.ContentType?.MediaType;
                var extension = contentType switch
                {
                    "image/jpeg" => ".jpg",
                    "image/png" => ".png",
                    "image/webp" => ".webp",
                    _ => ".jpg"
                };

                var uploadFolder = Path.Combine(_webRootPath, "upload", "images", "avatars");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                var fileName = $"{userId}{extension}";
                var filePath = Path.Combine(uploadFolder, fileName);

                await using var file = new FileStream(filePath, FileMode.Create);
                await response.Content.CopyToAsync(file,ct);

                return $"/upload/images/avatars/{fileName}";
            }
            catch
            {
                return null;
            }
        }
    }
}
