using System.Linq;
using System.Threading.Tasks;
using Carvana;
using LiteNotifications.WebApi.Infrastructure.Sql;

namespace LiteNotifications.WebApi.Persistence
{
    public sealed class SqlJsonIoTable : AsyncIo
    {
        private readonly DapperSqlDb _db;
        private readonly string _tableName;

        public SqlJsonIoTable(DapperSqlDb db, string tableName)
        {
            _db = db;
            _tableName = tableName;
        }

        public Task<Result<Unit>> Put<T>(string key, T data)
            => Delete(key)
                .Then(_ => _db.Execute($@"INSERT INTO {_tableName} (Id, Value) VALUES (@key, @data)", new {key, data = Json.Serialize(data)})
                .Then(() => new Unit()));

        public Task<Result<T>> Get<T>(string key)
            => _db.QuerySingle<T>($@"SELECT * FROM {_tableName} WHERE Id = @key", new {key});

        public Task<Result<bool>> Contains(string key) 
            => _db.Query<string>($@"SELECT Id FROM {_tableName} WHERE Id = @key", new {key}).Then(x => x.Any());
        
        public Task<Result<Unit>> Delete(string key) 
            => _db.Execute($@"DELETE FROM {_tableName} WHERE Id = @key", new { key }).Then(() => new Unit());
    }
}
