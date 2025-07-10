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

        string? message = await _blobStorageService.UploadFileInChunksAsync(file, chunkIndex, totalChunks);

        return Ok(new { message });
    }
}
