using System;
using System.Collections.Generic;

namespace LiteNotifications.WebApi.Domain
{
    public class Subscription
    {
        public string Topic { get; set; }
        public string GroupId { get; set; }
        public string OutletGroup { get; set; }
        
        public override bool Equals(object obj)
        {
            if (!(obj is Subscription sub)) return false;
            return Topic.Equals(sub.Topic, StringComparison.InvariantCultureIgnoreCase)
                   && OutletGroup.Equals(sub.OutletGroup, StringComparison.InvariantCultureIgnoreCase)
                   && GroupId.Equals(sub.GroupId, StringComparison.InvariantCultureIgnoreCase);
        }
        
        public override int GetHashCode()
        {
            return (Topic.ToLowerInvariant() + GroupId.ToLowerInvariant() + OutletGroup.ToLowerInvariant()).GetHashCode();
        }
    }
    
    public sealed class Subscriptions : List<Subscription>
    {
    }
}
