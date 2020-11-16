using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LiteNotifications.WebApi.Infrastructure.Slack
{
    public class SlackPostMessage : IPost<SlackMessage>
    {
        readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        readonly SlackClient _client;
        Dictionary<string, string> _channelsByName;

        public SlackPostMessage(SlackClient client)
        {
            _client = client;
        }

        public async Task Post(SlackMessage value)
        {
            var response = (await _client.Post<SlackPostMessageResponse>("chat.postMessage",
                new
                {
                    channel = value.Channel, // TODO: where from?
                    text = $"{value.Title} - {value.Text}", //TODO: improve formatting
                    link_names = true,
                    parse = "full",
                    icon_emoji = ":tada:" //TODO: can use `icon_url` for a more custom look...
                }));
        }

        class SlackPostMessageResponse
        {
            bool ok { get; set; }
            string ts { get; set; }
        }
    }
}
