using FileService.Services.Interfaces;

namespace FileService.Services;

public class FileService : IFileService
{
    private readonly IBlobStorageService _blobStorageService;

    public FileService(IBlobStorageService blobStorageService)
    {
        _blobStorageService = blobStorageService;
    }

    public async Task<string> UploadChunkedFile(IFormFile file, int chunkIndex, int totalChunks)
    {
        var uploadId = file.FileName;
        var tempPath = Path.Combine(Path.GetTempPath(), "uploads", uploadId);

        Directory.CreateDirectory(tempPath);

        var chunkPath = Path.Combine(tempPath, $"{chunkIndex}.chunk");
        using (var stream = new FileStream(chunkPath, FileMode.Create, FileAccess.Write))
        {
            await file.CopyToAsync(stream);
        }

        // If this is the last chunk, assemble the file
        if (chunkIndex == totalChunks - 1)
        {
            var finalFilePath = Path.Combine(tempPath, uploadId);
            using (var finalStream = new FileStream(finalFilePath, FileMode.Create, FileAccess.Write))
            {
                for (int i = 0; i < totalChunks; i++)
                {
                    var partPath = Path.Combine(tempPath, $"{i}.chunk");
                    using (var partStream = new FileStream(partPath, FileMode.Open, FileAccess.Read))
                    {
                        await partStream.CopyToAsync(finalStream);
                    }
                    File.Delete(partPath);
                }
            }

            string fileInfo;

            using (var assembledStream = new FileStream(finalFilePath, FileMode.Open, FileAccess.Read))
            {
                var formFile = new FormFile(assembledStream, 0, assembledStream.Length, "file", uploadId);
                fileInfo = await _blobStorageService.UploadFileAsync(formFile);
            }

            File.Delete(finalFilePath);

            return $"File uploaded successfully: {fileInfo}";
        }

        return $"Chunk {chunkIndex + 1}/{totalChunks} uploaded.";
    }
}
