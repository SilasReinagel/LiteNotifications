using System.Threading.Tasks;
using Carvana;
using LiteMediator;
using LiteNotifications.WebApi.Contracts;
using LiteNotifications.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;

namespace LiteNotifications.WebApi.Controllers
{
    [Route("api/[controller]")]
    public sealed class ManageController : Controller
    {
        private readonly AsyncMediator _handler;

        public ManageController(AsyncMediator handler) => _handler = handler;

        [HttpGet, Route("channelTypes")] public IActionResult GetAllChannelType([FromServices]Channels channels) => Result.Success(channels.Keys).AsResponse();
        [HttpPost, Route("outlet")] public Task<IActionResult> AddOutlet([FromBody]AddOutletRequest req) => _handler.Handle(req);
        [HttpDelete, Route("outlet")] public Task<IActionResult> RemoveOutlets([FromBody]RemoveOutletRequest req) => _handler.Handle(req);
        
        // TODO: Implement
//        [HttpGet, Route("outlets")] public IActionResult GetOutletsForGroup([FromServices]UserOutletsPersistence outlets, [FromQuery]string groupId)
//        {
//            return Ok(outlets.Get()[groupId.ToWebSafeBase64()]);
//        }
    }
}
