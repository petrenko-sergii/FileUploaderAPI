using Microsoft.AspNetCore.Mvc;

namespace FileUploaderAPI.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private const long MaxFileSize = 10L * 1024L * 1024L * 1024L; // 10GB

        private readonly IMultipartContentValidator _multipartContentValidator;
        private readonly IClientService _clientService;

        public UploadController(
            IMultipartContentValidator multipartContentValidator,
            IClientService clientService)
        {
            _multipartContentValidator = multipartContentValidator;
            _clientService = clientService;
        }

        [HttpGet]
        public string Get()
        {
            return "UploadController works ok";
        }

        [HttpPost]
        [DisableFormValueModelBinding]
        [RequestSizeLimit(MaxFileSize)]
        [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
        public async Task ReceiveFile()
        {
            var file = await _multipartContentValidator.ValidateAndExtractFileAsync(Request.ContentType, Request.Body);
            await _clientService.ForwardFileToFileServiceAsync(file.Stream, file.Name);
        }
    }
}
