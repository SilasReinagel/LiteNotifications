using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiteNotifications.WebApi._Common;

namespace LiteNotifications.WebApi.Infrastructure.Slack
{
    public class SlackUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class SlackGetUser : IExternal<string, Maybe<SlackUser>>
    {
        readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        readonly SlackClient _client;
        Dictionary<string, string> _usersByName;

        public SlackGetUser(SlackClient client)
        {
            _client = client;
        }

        public async Task<Maybe<SlackUser>> Get(string name)
        {
            if (_usersByName == null) await LoadUsers();
            if (_usersByName == null) return Maybe<SlackUser>.Missing;

            if (_usersByName.TryGetValue(name, out var channel))
                return new Maybe<SlackUser>(new SlackUser { Id = channel, Name = name });

            return Maybe<SlackUser>.Missing;
        }

        async Task LoadUsers()
        {
            //TODO: this is slow as hell, conside caching this in a jsonstore.io (without email)
            await _semaphoreSlim.WaitAsync();
            try
            {
                if (_usersByName != null) return; //TODO: invalidate
                var cursor = default(string);
                IEnumerable<SlackUserResponse.Member> list = new List<SlackUserResponse.Member>();

                do
                {
                    var users = (await _client.Get<SlackUserResponse>("users.list", new
                    {
                        include_locale = false,
                        limit = 500,
                        cursor = cursor
                    }));

                    if (users.ok)
                        list = list.Concat(users.members);

                    cursor = users.response_metadata.next_cursor;
                } while (!string.IsNullOrEmpty(cursor));

                //TODO: read until next_cursor is empty string (limit of 500 ~= 1mb)
                _usersByName = list.ToDictionary(k => k.name, v => v.id);
            }
            catch (Exception exception)
            {
                
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        class SlackUserResponse
        {
            public bool ok { get; set; }
            public List<Member> members { get; set; }
            public ResponseMetadata response_metadata { get; set; }

            public class Member
            {
                public string id { get; set; }
                public string name { get; set; }
                public MemberProfile profile { get; set; }
            }

            public class MemberProfile
            {
                public string email { get; set; }
            }

            public class ResponseMetadata
            {
                public string next_cursor { get; set; }
            }
        }
    }
}
