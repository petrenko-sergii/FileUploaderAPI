using FileService.Services.Interfaces;

namespace FileService.Services;

public class FileService : IFileService
{
    private readonly IBlobStorageService _blobStorageService;

    public FileService(IBlobStorageService blobStorageService)
    {
        _blobStorageService = blobStorageService;
    }

    public async Task<string?> UploadChunkedFile(IFormFile file, int chunkIndex, int totalChunks)
    {
        return await _blobStorageService.UploadFileInChunksAsync(file, chunkIndex, totalChunks);
    }
}
