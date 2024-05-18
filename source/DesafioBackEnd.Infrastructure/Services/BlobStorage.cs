using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using DesafioBackEnd.Domain.Contracts.Services;

namespace DesafioBackEnd.Infrastructure.Services;

public class BlobStorage : IStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private const string _containerName = "desafiobackend";

    public BlobStorage(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }

    public async Task<string?> UploadFileAsync(Stream file, string prefix, string fileExtension, string fileContentType)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

        if (!containerClient.Exists())
        {
            containerClient = await _blobServiceClient.CreateBlobContainerAsync(_containerName, PublicAccessType.BlobContainer);
        }

        var fileName = $"{prefix}/{Guid.NewGuid()}.{fileExtension}";

        BlobClient blobClient = containerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(file, new BlobHttpHeaders { ContentType = fileContentType });

        return fileName;
    }

    public async Task DeleteFileIfExistsAsync(string fileName, CancellationToken cancellationToken = default)
    {
        BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

        BlobClient blobClient = containerClient.GetBlobClient(fileName);

        await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }
}
