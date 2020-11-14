using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiteNotifications.WebApi.Domain
{
    public interface IChannel
    {
        string Type { get; }
        Task Send(RoutedNotification n);
    }
    
    public class Channels : Dictionary<string, IChannel>
    {
        public Channels() : base(StringComparer.InvariantCultureIgnoreCase) { }
    }
}
