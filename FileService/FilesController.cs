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
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file was uploaded.");
        }

        var fileUrl = await _blobStorageService.UploadFileAsync(file);
        return Ok(new { message = $"File uploaded successfully: {fileUrl}" });
    }
}
