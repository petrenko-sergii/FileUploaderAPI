using FileUploaderAPI.Server.Interfaces;
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
        private readonly IProgressBarHelper _progressBarHelper;

        public UploadController(
            IMultipartContentValidator multipartContentValidator,
            IClientService clientService,
            IProgressBarHelper progressBarHelper)
        {
            _multipartContentValidator = multipartContentValidator;
            _clientService = clientService;
            _progressBarHelper = progressBarHelper;
        }

        [HttpPost]
        [DisableFormValueModelBinding]
        [RequestSizeLimit(MaxFileSize)]
        [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
        public async Task Upload()
        {
            var file = await _multipartContentValidator.ValidateAndExtractFileAsync(Request.ContentType, Request.Body);

            await _progressBarHelper.SendProgressBarData(file, Request.ContentLength);
            await _clientService.ForwardFileToFileServiceAsync(file.Stream, file.Name);
        }
    }
}
