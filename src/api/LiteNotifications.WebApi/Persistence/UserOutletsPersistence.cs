using System;
using System.Collections.Generic;
using System.Linq;
using LiteNotifications.WebApi._Common;
using LiteNotifications.WebApi.Contracts;
using LiteNotifications.WebApi.Domain;

namespace LiteNotifications.WebApi.Persistence
{
    public sealed class UserOutletsPersistence : IValue<UserOutlets>
    {
        private readonly Io _io;
        private readonly string _resourceName;

        public UserOutletsPersistence(Io io, string resourceName = null)
        {
            _io = io;
            _resourceName = resourceName ?? "UserOutlets";
        }

        public UserOutlets Get() => _io.GetInitialized(_resourceName, () => new UserOutlets());

        public void Add(AddOutletRequest req)
        {
            var userOutlets = Get();

            var userId = req.UserId.ToWebSafeBase64();
            if (!userOutlets.ContainsKey(userId))
                userOutlets[userId] = new List<UserOutletData>();

            userOutlets[userId].Add(new UserOutletData
            {
                Id = Guid.NewGuid().ToString(),
                OutletGroup = req.OutletGroup,
                OutletType = req.OutletType,
                Target = req.Target
            });

            userOutlets[userId] = userOutlets[userId].Distinct().ToList(); //TODO: quick hack to also fix users with dupes users

            _io.Put(_resourceName, userOutlets);
        }

        public void Remove(RemoveOutletRequest req)
        {
            var userOutlets = Get();

            var userId = req.UserId.ToWebSafeBase64();
            if (!userOutlets.ContainsKey(userId)) return;

            userOutlets[userId] = userOutlets[userId].Except(userOutlets[userId].Where(w => w.Id == req.OutletId)).ToList();

            _io.Put(_resourceName, userOutlets);
        }
    }
}
