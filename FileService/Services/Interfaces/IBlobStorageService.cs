namespace FileService.Services.Interfaces;

public interface IBlobStorageService
{
    Task<string?> UploadFileInChunksAsync(IFormFile fileChunk, int chunkIndex, int totalChunks);
}
