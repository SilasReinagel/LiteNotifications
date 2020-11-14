using System.Threading.Tasks;
using LiteNotifications.WebApi.Domain;

namespace LiteNotifications.WebApi.Infrastructure.Slack
{

    public class SlackChannelChannel : IChannel
    {
        public string Type => "SlackChannel";
        SlackGetChannel _channels;
        SlackPostMessage _post;
        public SlackChannelChannel(SlackPostMessage post, SlackGetChannel channels)
        {
            _post = post;
            _channels = channels;
        }
        public async Task Send(RoutedNotification notification)
        {
            var channelId = await _channels.Get(notification.Target);

            if (channelId.HasValue)
                await _post.Post(new SlackMessage
                {
                    Channel = channelId.Value.Id,
                    Text = notification.Text,
                });
        }
    }
}
