using System.Threading.Tasks;
using LiteNotifications.WebApi.Domain;

namespace LiteNotifications.WebApi.Infrastructure.Slack
{
    public class SlackDestination : Destination
    {
        public string Type => "SlackChannel";
        SlackGetChannel _channels;
        SlackPostMessage _post;
        public SlackDestination(SlackPostMessage post, SlackGetChannel channels)
        {
            _post = post;
            _channels = channels;
        }
        public async Task Send(RoutedNotification notification)
        {
            var channelId = await _channels.Get(notification.Target);

            if (channelId.Succeeded() && channelId.Content.HasValue)
                await _post.Post(new SlackMessage
                {
                    Channel = channelId.Content.Value.Id,
                    Text = notification.Text,
                });
        }
    }
}
