using Microsoft.AspNetCore.Mvc;

namespace FileService;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
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

        return Ok(new { message = "File uploaded successfully." });
    }
}
