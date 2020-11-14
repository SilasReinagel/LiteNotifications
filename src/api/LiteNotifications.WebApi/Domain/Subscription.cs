using System;
using System.Collections.Generic;

namespace LiteNotifications.WebApi.Domain
{
    public class Subscription
    {
        public string Topic { get; set; }
        public string UserId { get; set; }
        public string OutletGroup { get; set; }
        
        public override bool Equals(object obj)
        {
            if (!(obj is Subscription sub)) return false;
            return Topic.Equals(sub.Topic, StringComparison.InvariantCultureIgnoreCase)
                   && OutletGroup.Equals(sub.OutletGroup, StringComparison.InvariantCultureIgnoreCase)
                   && UserId.Equals(sub.UserId, StringComparison.InvariantCultureIgnoreCase);
        }
        
        public override int GetHashCode()
        {
            return (Topic.ToLowerInvariant() + UserId.ToLowerInvariant() + OutletGroup.ToLowerInvariant()).GetHashCode();
        }
    }
    
    public sealed class Subscriptions : List<Subscription>
    {
    }
}
