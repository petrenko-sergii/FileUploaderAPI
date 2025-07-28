namespace FileService.Services.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadStreamAsync(Stream stream, string fileName);
}
