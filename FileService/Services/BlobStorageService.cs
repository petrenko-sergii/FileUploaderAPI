using Azure.Storage.Blobs;
using FileService.Services.Interfaces;

namespace FileService.Services;

public class BlobStorageService(
    BlobServiceClient blobServiceClient, 
    INotifyService notifyService) : IBlobStorageService
{
    private const string ContainerName = "largefilescontainer";

    public async Task<string> UploadStreamAsync(Stream stream, string fileName, long fileLength)
    {
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);

        await containerClient.CreateIfNotExistsAsync();
        await containerClient.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

        var blobClient = containerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(stream, overwrite: true);

        var fileInfo = new FileInfo
        {
            Name = fileName,
            Size = fileLength,
            Uri = blobClient.Uri.ToString()
        };

        await notifyService.NotifyFileUploadedAsync(fileInfo);

        return $"File \"{fileName}\" uploaded successfully";
    }
}