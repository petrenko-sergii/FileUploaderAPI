using Azure.Storage.Blobs;
using FileService.Config;
using FileService.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace FileService.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobStorageOptions _blobStorageOptions;

    public BlobStorageService(IOptions<BlobStorageOptions> blobStorageOptions)
    {
        _blobStorageOptions = blobStorageOptions.Value;
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        BlobContainerClient containerClient = new BlobContainerClient(
            _blobStorageOptions.ConnectionString,
            _blobStorageOptions.ContainerName);

        await containerClient.CreateIfNotExistsAsync();

        await containerClient.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.None);

        var blobClient = containerClient.GetBlobClient(file.FileName);

        await using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, true);

        return blobClient.Uri.ToString();
    }
}