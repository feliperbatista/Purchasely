using Purchasely.Domain.Entities;

namespace Purchasely.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadAsync(Stream stream, string fileName, string contentType, CancellationToken cancellationToken);
    Task DeleteAsync(string blobUrl, CancellationToken cancellationToken);
}