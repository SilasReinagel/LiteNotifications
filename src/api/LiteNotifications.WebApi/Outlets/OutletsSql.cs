using System.Threading.Tasks;
using Carvana;
using LiteNotifications.WebApi.Contracts;
using LiteNotifications.WebApi.Infrastructure.Sql;

namespace LiteNotifications.WebApi.Outlets
{
    public sealed class OutletsSql : IExternal<AddOutletRequest, Unit>, IExternal<RemoveOutletRequest, Unit>
    {
        private readonly DapperSqlDb _db;

        public OutletsSql(DapperSqlDb db) => _db = db;

        public Task<Result<Unit>> Get(AddOutletRequest key)
            => _db.ExecuteUnit(@"
                    IF NOT EXISTS (SELECT * FROM NotifyApp.Outlets WHERE Hash = @hash)
                    INSERT INTO NotifyApp.Outlets (GroupId, OutletType, Target, Hash) VALUES (@groupId, @outletType, @target, @hash)", key);

        public Task<Result<Unit>> Get(RemoveOutletRequest key)
            => _db.ExecuteUnit(@"DELETE FROM NotifyApp.Outlets WHERE Id = @id}", new { id = key.OutletId });
    }
}
