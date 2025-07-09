namespace FileService.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadChunkedFile(IFormFile file, int chunkIndex, int totalChunks);
    }
}