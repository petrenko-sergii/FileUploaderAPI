namespace FileUploaderAPI.Server.Interfaces
{
    public interface IBlobStorageService
    {
        Task UploadFileAsync(IFormFile file);
    }
}