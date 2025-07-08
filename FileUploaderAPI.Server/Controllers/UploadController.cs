using FileUploaderAPI.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FileUploaderAPI.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<UploadController> _logger;
        private readonly IBlobStorageService _blobStorageService;

        public UploadController(ILogger<UploadController> logger, IBlobStorageService blobStorageService)
        {
            _logger = logger;
            _blobStorageService = blobStorageService;
        }

        [HttpGet]
        public IEnumerable<File> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new File
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file was uploaded.");
            }

            await _blobStorageService.UploadFileAsync(file);
            return Ok(new { message = "File uploaded successfully." });
        }
    }
}
