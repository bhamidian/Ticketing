namespace Ticketing.Application.Contracts.Services;

public interface IFileService
{
    Task<string?> UploadAsync(
        Stream content,
        string fileName,
        string folder,
        CancellationToken cancellationToken = default);

    Stream? OpenRead(string fileName);
    void Delete(string fileName);
}
