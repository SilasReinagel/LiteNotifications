﻿using System.Threading.Tasks;
using LiteNotifications.WebApi.Domain;

namespace LiteNotifications.WebApi.Infrastructure.Slack
{
    public class SlackUserChannel : IChannel
    {
        public string Type => "SlackUser";
        SlackGetUser _users;
        SlackPostMessage _post;
        public SlackUserChannel(SlackPostMessage post, SlackGetUser users)
        {
            _post = post;
            _users = users;
        }

        public async Task Send(RoutedNotification notification)
        {
            var userId = await _users.Get(notification.Target);
            if (userId.HasValue)
                await _post.Post(new SlackMessage
                {
                    Channel = userId.Value.Id,
                    Title = notification.Title,
                    Text = notification.Text,
                });
        }
    }
}