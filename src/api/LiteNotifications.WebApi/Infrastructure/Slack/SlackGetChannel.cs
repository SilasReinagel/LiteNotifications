using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiteNotifications.WebApi._Common;

namespace LiteNotifications.WebApi.Infrastructure.Slack
{
    public class SlackChannel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class Maybe<T>
    {
        public T Value { get; }
        public Maybe(T value)
        {
            Value = value;
        }

        public static Maybe<T> Missing = new Maybe<T>(default(T));

        public bool HasValue => !Equals(Value, default(T));
    }

    public class SlackGetChannel : IExternal<string, Maybe<SlackChannel>>
    {
        readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        readonly SlackClient _client;
        Dictionary<string, string> _channelsByName;

        public SlackGetChannel(SlackClient client)
        {
            _client = client;
        }

        public async Task<Maybe<SlackChannel>> Get(string name)
        {
            if (_channelsByName == null) await LoadChannels();
            if (_channelsByName == null) return Maybe<SlackChannel>.Missing;

            if (_channelsByName.TryGetValue(name, out var channel))
                return new Maybe<SlackChannel>(new SlackChannel { Id = channel, Name = name });

            return Maybe<SlackChannel>.Missing;
        }

        async Task LoadChannels()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                if (_channelsByName != null) return; //TODO: invalidate

                _channelsByName = (await _client.Get<SlackChannelResponse>("channels.list"))
                    ?.channels.ToDictionary(k => k.name, v => v.name);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        class SlackChannelResponse
        {
            public bool ok { get; set; }
            public List<Channel> channels { get; set; }

            public class Channel
            {
                public string id { get; set; }
                public string name { get; set; }
            }
        }
    }
}
