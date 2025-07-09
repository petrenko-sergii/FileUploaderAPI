namespace FileService.Services.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadFileAsync(IFormFile file);
}
