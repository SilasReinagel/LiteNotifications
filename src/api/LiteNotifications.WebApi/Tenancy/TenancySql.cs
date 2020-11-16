using System.Threading.Tasks;
using Carvana;
using LiteNotifications.WebApi.Infrastructure.Sql;

namespace LiteNotifications.WebApi.Tenancy
{
    public class CreateGroupRequest
    {
        public string GroupName { get; set; }
    }

    public class AddUserToGroupRequest
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
    }

    public sealed class TenancySql : IExternal<CreateGroupRequest, int>, IExternal<AddUserToGroupRequest, Unit>
    {
        private readonly DapperSqlDb _db;

        public TenancySql(DapperSqlDb db) => _db = db;

        public Task<Result<int>> Get(CreateGroupRequest key) =>
            _db.QuerySingle<int>(@"INSERT INTO NotifyApp.Groups(GroupDisplayName) VALUES (@groupDisplayName) SELECT SCOPE_IDENTITY()",
                new {groupDisplayName = $"{key.GroupName}"});

        public Task<Result<Unit>> Get(AddUserToGroupRequest req)
            => _db.Execute(@"INSERT INTO NotifyApp.UserGroups(UserId, GroupId) VALUES (@userId, @groupId)", new {userId = req.UserId, groupId = req.GroupId})
                .Then(() => new Unit());
    }
}
