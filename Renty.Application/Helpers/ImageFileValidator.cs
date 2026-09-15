using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Org.BouncyCastle.Asn1.Ocsp;
using Renty.Application.Common;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Renty.Application.Helpers
{
    public static class ImageFileValidator
    {
        static private string[] imageTypes = new string[] { ".jpg", ".jpeg", ".png", ".webp" };
        static public bool IsFileTypeImage(string fileName) => imageTypes.Contains(Path.GetExtension(fileName).ToLowerInvariant());

        static public OperationResult<Unit> ValidateFile(IFormFile file, int maxFileSize = 25 * 1024 * 1024)
        {
            if (file == null)
                return OperationResult<Unit>.Fail("File is null");

            if (file.Length > maxFileSize)
                return OperationResult<Unit>.Fail($"File size exceeds the limit {Math.Round((decimal)maxFileSize / (1024 * 1024),1)}MB");

            if (file.Length == 0)
                return OperationResult<Unit>.Fail("File is empty");

            if (IsFileTypeImage(file.FileName))
                return OperationResult<Unit>.Fail("The file is not an image");

            return OperationResult<Unit>.Success(new Unit());
        }
    }
}
