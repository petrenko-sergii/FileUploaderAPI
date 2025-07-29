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

    [HttpPost]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> Upload()
    {
        var fileName = Request.Headers["X-File-Name"].FirstOrDefault();
        if (string.IsNullOrEmpty(fileName))
        {
            return BadRequest("Missing X-File-Name header.");
        }

        long.TryParse(Request.Headers["X-File-Length"].FirstOrDefault(), out long fileLength);
        if (fileLength == 0)
        {
            return BadRequest("Header X-File-Length is missing or invalid.");
        }

        var message = await _blobStorageService.UploadStreamAsync(Request.Body, fileName, fileLength);

        return Ok(new { message });
    }
}
