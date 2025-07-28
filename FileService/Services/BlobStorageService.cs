using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using FileService.Services.Interfaces;
using System.Text;

namespace FileService.Services;

public class BlobStorageService(
    BlobServiceClient blobServiceClient, 
    INotifyService notifyService) : IBlobStorageService
{
    private const string ContainerName = "largefilescontainer";

    public async Task<string> UploadStreamAsync(Stream stream, string fileName)
    {
        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);

        await containerClient.CreateIfNotExistsAsync();
        await containerClient.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

        var blobClient = containerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(stream, overwrite: true);

        var blobSize = (await blobClient.GetPropertiesAsync()).Value.ContentLength;

        var fileInfo = new FileInfo
        {
            Name = fileName,
            Size = blobSize,
            Uri = blobClient.Uri.ToString()
        };

        await notifyService.NotifyFileUploadedAsync(fileInfo);

        return $"File \"{fileName}\" uploaded successfully";
    }
}