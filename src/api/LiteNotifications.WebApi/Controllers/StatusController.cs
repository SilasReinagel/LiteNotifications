using Microsoft.AspNetCore.Mvc;

namespace LiteNotifications.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class StatusController : Controller
    {
        [HttpGet]
        public IActionResult GetStatus()
        {
            return new ContentResult() { Content = "<html><body><h1>Service is live!</h1></body></html>", ContentType = "text/html" };
        }
    }
}
