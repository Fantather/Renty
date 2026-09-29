using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.Interfaces
{
    public interface IAvatarDownloadService
    {
        Task<string?> DownloadAndSaveAsync(string sourceUrl, Guid userId, CancellationToken ct = default);
    }
}
