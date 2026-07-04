using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Purchasely.Application.Interfaces;

namespace Purchasely.Infrastructure.Services;

public class AzureBlobStorageService(IConfiguration configuration) : IFileStorageService
{
    private readonly string _connectionString = configuration["AzureBlobStorage:ConnectionString"]!;
    private readonly string _containerName = configuration["AzureBlobStorage:ContainerName"]!;
    
    private BlobContainerClient GetContainerClient()
    {
        var client = new BlobContainerClient(_connectionString, _containerName);
        client.CreateIfNotExists(Azure.Storage.Blobs.Models.PublicAccessType.None);
        return client;
    }

    public async Task<string> UploadAsync(Stream stream, string fileName, string contentType, CancellationToken cancellationToken)
    {
        var container = GetContainerClient();
        var blobName = $"{Guid.NewGuid()}_{fileName}";
        var blobClient = container.GetBlobClient(blobName);

        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: cancellationToken);

        return blobClient.Uri.ToString();
    }

    public async Task DeleteAsync(string blobUrl, CancellationToken cancellationToken)
    {
        var container = GetContainerClient();
        var blobName = new Uri(blobUrl).Segments.Last();
        var blobClient = container.GetBlobClient(blobName);

        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }
}