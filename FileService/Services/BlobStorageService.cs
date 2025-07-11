using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using FileService.Config;
using FileService.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Text;

namespace FileService.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobStorageOptions _blobStorageOptions;
    private readonly INotifyService _notifyService;

    public BlobStorageService(
        IOptions<BlobStorageOptions> blobStorageOptions, 
        INotifyService notifyService)
    {
        _blobStorageOptions = blobStorageOptions.Value;
        _notifyService = notifyService;
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

        return $"Name \"{file.FileName}\" with size {file.Length} B. URI: {blobClient.Uri.ToString()}";
    }

    public async Task<string?> UploadFileInChunksAsync(IFormFile fileChunk, int chunkIndex, int totalChunks)
    {
        BlobContainerClient containerClient = new BlobContainerClient(
            _blobStorageOptions.ConnectionString,
            _blobStorageOptions.ContainerName);

        await containerClient.CreateIfNotExistsAsync();

        await containerClient.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

        var blobName = fileChunk.FileName;
        var blockBlobClient = containerClient.GetBlockBlobClient(blobName);

        using (var fileStream = fileChunk.OpenReadStream())
        {
            var blockId = Convert.ToBase64String(Encoding.UTF8.GetBytes(chunkIndex.ToString("d6")));

            await blockBlobClient.StageBlockAsync(blockId, fileStream);

            if (chunkIndex == totalChunks - 1)
            {
                var blockList = Enumerable.Range(0, totalChunks)
                    .Select(i => Convert.ToBase64String(Encoding.UTF8.GetBytes(i.ToString("d6")))).ToList();

                await blockBlobClient.CommitBlockListAsync(blockList);
                var blobSize = (await blockBlobClient.GetPropertiesAsync()).Value.ContentLength;

                var fileInfo = new FileInfo
                {
                    Name = blobName,
                    Size = blobSize,
                    Uri = blockBlobClient.Uri.ToString()
                };

                await _notifyService.NotifyFileUploadedAsync(fileInfo);

                return $"File \"{blobName}\" uploaded successfully";
            }
        }

        return null;
    }
}