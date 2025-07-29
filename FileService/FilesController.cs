using FileService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FileService;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly IBlobStorageService _blobStorageService;
    private readonly IHeadersHelper _headersHelper;

    public FilesController(
        IBlobStorageService blobStorageService, IHeadersHelper headersHelper)
    {
        _blobStorageService = blobStorageService;
        _headersHelper = headersHelper;
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> Upload()
    {
        string fileName = _headersHelper.GetFileName(Request.Headers);
        long fileLength = _headersHelper.GetFileLength(Request.Headers);

        var message = await _blobStorageService.UploadStreamAsync(Request.Body, fileName, fileLength);

        return Ok(new { message });
    }
}
