using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Ticketing.Application.Contracts.Services;

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

        public async Task<string?> UploadAsync(
            Stream content,
            string fileName,
            string folder,
            CancellationToken cancellationToken = default)
        {
            if (content is null || !content.CanRead)
            {
                logger.LogWarning("File upload skipped because the content stream was not readable.");
                return null;
            }

            var uploadsFolder = ResolveWebRootPath(Path.Combine("Teams", folder));
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            await using (var output = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                await content.CopyToAsync(output, cancellationToken);

            var webPath = $"/Teams/{folder}/{uniqueFileName}";
            logger.LogInformation(
                "File uploaded. {Folder} {FileName} {SizeBytes}",
                folder,
                uniqueFileName,
                new FileInfo(filePath).Length
            );
            return webPath;
        }

        public Stream? OpenRead(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return null;

            var fullPath = ResolveWebRootPath(fileName);
            if (!File.Exists(fullPath))
                return null;

            return new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
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
