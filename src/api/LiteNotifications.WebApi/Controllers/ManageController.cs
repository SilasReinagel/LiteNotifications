using LiteNotifications.WebApi.Contracts;
using LiteNotifications.WebApi.Domain;
using LiteNotifications.WebApi.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace LiteNotifications.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ManageController : Controller
    {
        [HttpGet, Route("ChannelTypes")]
        public IActionResult GetAllChannelType([FromServices] Channels channels)
        {
            return Ok(channels.Keys);
        }
        
        [HttpPost, Route("AddOutlet")]
        public IActionResult AddOutlet([FromServices]UserOutletsPersistence outlets, [FromBody]AddOutletRequest req)
        {
            outlets.Add(req);
            return Ok();
        }

        [HttpDelete, Route("RemoveOutlet")]
        public IActionResult RemoveOutlets([FromServices]UserOutletsPersistence outlets, [FromBody]RemoveOutletRequest req)
        {
            outlets.Remove(req);
            return Ok();
        }

        [HttpGet, Route("Outlets")]
        public IActionResult GetOutletsForUser([FromServices]UserOutletsPersistence outlets, string userId)
        {
            return Ok(outlets.Get()[userId.ToWebSafeBase64()]);
        }
    }
}
