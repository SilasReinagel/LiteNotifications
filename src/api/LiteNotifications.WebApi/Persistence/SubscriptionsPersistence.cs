﻿using System.Collections.Generic;
using System.Linq;
using LiteNotifications.WebApi.Contracts;
using LiteNotifications.WebApi.Domain;

namespace LiteNotifications.WebApi.Persistence
{
    public sealed class SubscriptionsPersistence : IValue<Subscriptions>
    {
        private readonly SimpleIo _io;
        private readonly string _resourceName;

        public SubscriptionsPersistence(SimpleIo io, string resourceName = null)
        {
            _io = io;
            _resourceName = resourceName ?? "Subscriptions";
        } 
        
        public Subscriptions Get() => _io.GetInitialized(_resourceName, () => new Subscriptions());

        public void Add(SubscriptionRequest req)
        {
            var subscriptions = Get();
            var newSub = new Subscription {Topic = req.Topic, GroupId = req.UserId, OutletGroup = req.OutletGroup};
            var updated = subscriptions.Concat(new List<Subscription> {newSub}).Distinct().ToList();
            _io.Put(_resourceName, updated);
        }

        public void Remove(SubscriptionRequest req)
        {
            Remove(req.UserId, req.Topic, req.OutletGroup);
        }

        public void Remove(string userId, string topic, string outletGroup)
        {
            var subscriptions = Get();
            var target = new Subscription {Topic = topic, GroupId = userId, OutletGroup = outletGroup};
            var subs = subscriptions.Where(s => !s.Equals(target)).ToList();
            _io.Put(_resourceName, subs);
        }
    }
}
