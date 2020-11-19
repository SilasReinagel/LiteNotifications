using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LiteNotifications.WebApi.Domain
{
    public interface Destination
    {
        string Type { get; }
        Task Send(RoutedNotification n);
    }
    
    public class Channels : Dictionary<string, Destination>
    {
        public Channels() : base(StringComparer.InvariantCultureIgnoreCase) { }
    }
}
