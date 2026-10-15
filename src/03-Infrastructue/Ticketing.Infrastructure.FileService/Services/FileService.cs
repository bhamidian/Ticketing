using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Ticketing.Infrastructure.FileService.Contracts;

namespace Ticketing.Infrastructure.FileService.Services
{
    public class FileService(IWebHostEnvironment environment, ILogger<FileService> logger)
        : IFileService
    {
        public void Delete(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                logger.LogDebug("File deletion skipped because the file name was empty.");
                return;
            }

            var fullPath = ResolveWebRootPath(fileName);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                logger.LogInformation(
                    "Uploaded file deleted. {FileName}",
                    Path.GetFileName(fileName)
                );
            }
            else
                logger.LogDebug(
                    "File deletion skipped because the file does not exist. {FileName}",
                    Path.GetFileName(fileName)
                );
        }

        public string? Upload(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
            {
                logger.LogWarning("File upload skipped because the uploaded file was empty.");
                return null;
            }

            var uploadsFolder = ResolveWebRootPath(Path.Combine("Teams", folder));
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var webPath = $"/Teams/{folder}/{uniqueFileName}";
            logger.LogInformation(
                "File uploaded. {Folder} {FileName} {SizeBytes}",
                folder,
                uniqueFileName,
                file.Length
            );
            return webPath;
        }

        private string ResolveWebRootPath(string relativePath)
        {
            var configuredWebRoot = environment.WebRootPath;
            var webRoot = Path.GetFullPath(
                string.IsNullOrWhiteSpace(configuredWebRoot)
                    ? Path.Combine(environment.ContentRootPath, "wwwroot")
                    : configuredWebRoot
            );
            Directory.CreateDirectory(webRoot);
            var normalizedRelativePath = relativePath
                .TrimStart('/', '\\')
                .Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.GetFullPath(Path.Combine(webRoot, normalizedRelativePath));
            var webRootPrefix = webRoot.EndsWith(Path.DirectorySeparatorChar)
                ? webRoot
                : webRoot + Path.DirectorySeparatorChar;

            if (!fullPath.StartsWith(webRootPrefix, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException(
                    "The file path must be inside the web root.",
                    nameof(relativePath)
                );

            return fullPath;
        }
    }
}
