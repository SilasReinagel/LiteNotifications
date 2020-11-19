using System.Threading.Tasks;
using LiteNotifications.WebApi.Domain;

namespace LiteNotifications.WebApi.Infrastructure.Slack
{
    public class SlackUserDestination : Destination
    {
        public string Type => "SlackUser";
        SlackGetUser _users;
        SlackPostMessage _post;
        public SlackUserDestination(SlackPostMessage post, SlackGetUser users)
        {
            _post = post;
            _users = users;
        }

        public async Task Send(RoutedNotification notification)
        {
            var userId = await _users.Get(notification.Target);
            if (userId.Succeeded() && userId.Content.HasValue)
                await _post.Post(new SlackMessage
                {
                    Channel = userId.Content.Value.Id,
                    Title = notification.Title,
                    Text = notification.Text,
                });
        }
    }
}
