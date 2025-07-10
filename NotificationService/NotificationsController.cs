using Microsoft.AspNetCore.Mvc;
using NotificationService.Services.Interfaces;

namespace NotificationService
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public NotificationsController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<string> Get()
        {
            return "String from \"NotificationService\"";
        }

        [HttpPost]
        public IActionResult SendEmail()
        {
            _emailService.SendEmail("file-details");
            return Ok("Email sent.");
        }
    }
}
