using FileService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FileService;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly IBlobStorageService _blobStorageService;

    public FilesController(IBlobStorageService blobStorageService)
    {
        _blobStorageService = blobStorageService;
    }

    [HttpGet]
    public async Task<string> Get()
    {
        return "String from \"FileService\"";
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> Upload(
        IFormFile file,
        [FromForm] int chunkIndex,
        [FromForm] int totalChunks)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file was uploaded.");
        }

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
                    System.IO.File.Delete(partPath);
                }
            }

            string fileInfo;

            using (var assembledStream = new FileStream(finalFilePath, FileMode.Open, FileAccess.Read))
            {
                var formFile = new FormFile(assembledStream, 0, assembledStream.Length, "file", uploadId);
                fileInfo = await _blobStorageService.UploadFileAsync(formFile);
            }

            System.IO.File.Delete(finalFilePath);

            return Ok(new { message = $"File uploaded successfully: {fileInfo}" });
        }

        return Ok(new { message = $"Chunk {chunkIndex + 1}/{totalChunks} uploaded." });
    }
}
