using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteNotifications.WebApi.Domain;

namespace LiteNotifications.WebApi.UseCases
{
    public sealed class UglyPublishNotificationFirstDraft
    {
        private readonly string _publicUrl;
        private readonly IValue<Subscriptions> _subs;
        private readonly Dictionary<string, IChannel> _channels;
        private readonly IValue<UserOutlets> _userOutlets;

        public UglyPublishNotificationFirstDraft(string publicUrl, IValue<Subscriptions> subs, IValue<UserOutlets> userUserOutlets, Channels channels)
        {
            _publicUrl = publicUrl;
            _subs = subs;
            _channels = channels;
            _userOutlets = userUserOutlets;
        }
        
        public Task Send(Notification n)
        {
            var subs = _subs.Get().Where(x => x.Topic.Equals(n.Topic, StringComparison.InvariantCultureIgnoreCase))
                .ToDictionary(x => x.UserId.ToWebSafeBase64(), x => x.OutletGroup, StringComparer.InvariantCultureIgnoreCase);
            var users = subs.Keys;
            
            var subscribedUsers = _userOutlets.Get().Where(x => users.Contains(x.Key, StringComparer.InvariantCultureIgnoreCase));
            foreach (var user in subscribedUsers)
            {
                user.Value.ForEach(o =>
                {
                    if (o.OutletGroup.Equals(subs[user.Key], StringComparison.InvariantCultureIgnoreCase))
                        Send(user.Key, n, o);
                });
            }
            return Task.CompletedTask;
        }

        private void Send(string userId, Notification n, UserOutletData o)
        {
            if (!_channels.ContainsKey(o.OutletType))
                throw new KeyNotFoundException($"Unsupported Outlet Type: {o.OutletType}");
            _channels[o.OutletType].Send(new RoutedNotification
            {
                Title = n.Title,
                Text = n.Text + GetUnsubscribeText(userId, n, o),
                Target = o.Target,
            });
        }

        private string GetUnsubscribeText(string userId, Notification n, UserOutletData o)
        {
            return
                $"\r\n\r\n---------------------------\r\nTo Unsubscribe Click - {_publicUrl}/api/notifications/unsubscribeMe?userId={userId}&topic={n.Topic}&outletGroup={o.OutletGroup}";
        }
    }
}
