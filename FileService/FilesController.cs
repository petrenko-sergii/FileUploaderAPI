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
}
