using Microsoft.AspNetCore.Mvc;

namespace NotificationService
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        [HttpGet]
        public async Task<string> Get()
        {
            return "String from \"NotificationService\"";
        }
    }
}
