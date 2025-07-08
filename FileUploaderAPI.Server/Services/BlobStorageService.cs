using FileUploaderAPI.Server.Interfaces;

namespace FileUploaderAPI.Server.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        public async Task UploadFileAsync(IFormFile file)
        {
            // Implementation for uploading a file to blob storage
            return;
        }
    }
}
