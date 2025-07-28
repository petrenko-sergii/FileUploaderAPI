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

        var message = await _blobStorageService.UploadStreamAsync(Request.Body, fileName);

        return Ok(new { message });
    }
}
