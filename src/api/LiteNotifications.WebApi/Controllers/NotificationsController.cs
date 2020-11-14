using System;
using System.Linq;
using LiteNotifications.WebApi.Contracts;
using LiteNotifications.WebApi.Domain;
using LiteNotifications.WebApi.Persistence;
using LiteNotifications.WebApi.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace LiteNotifications.WebApi.Controllers
{
    [Route("api/[controller]")]
    public sealed class NotificationsController : Controller
    {
        [HttpPost, Route("publish")]
        public IActionResult PublishNotification([FromServices]UglyPublishNotificationFirstDraft pub, [FromBody]Notification notification)
        {
            pub.Send(notification);
            return Ok();
        }

        [HttpPost, Route("subscribe")]
        public IActionResult SubscribeToNotification([FromServices]SubscriptionsPersistence subscriptions, [FromBody]SubscriptionRequest req)
        {
            subscriptions.Add(req);
            return Ok();
        }
        
        [HttpGet, Route("subscriptions")]
        public IActionResult GetSubscriptions([FromServices]SubscriptionsPersistence subs, string userId)
        {
            return Ok(subs.Get().Where(x => x.UserId.Equals(userId, StringComparison.InvariantCultureIgnoreCase)).ToDictionary(x => x.Topic, x => x.OutletGroup, StringComparer.InvariantCultureIgnoreCase));
        }

        [HttpPost, Route("unsubscribe")]
        public IActionResult UnsubscribeFromNotification([FromServices] SubscriptionsPersistence subscriptions,
            [FromBody] SubscriptionRequest req)
        {
            subscriptions.Remove(req);
            return Ok($"Successfully Unsubscribed from Notification Topic {req.Topic}");
        }

        [HttpGet, Route("unsubscribeMe")]
        public IActionResult UnsubscribeMeLink([FromServices]SubscriptionsPersistence subscriptions, [FromQuery] string userId, [FromQuery] string topic, [FromQuery] string outletGroup)
        {
            subscriptions.Remove(userId, topic, outletGroup);
            return Ok();
        }
    }
}
