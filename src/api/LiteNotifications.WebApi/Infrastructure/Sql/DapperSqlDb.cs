using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Carvana;
using Dapper;

namespace LiteNotifications.WebApi.Infrastructure.Sql
{
    public sealed class DapperSqlDb
    {
        public async Task<bool> IsHealthy()
            => (await this.QuerySingle<int>("SELECT 1")).Succeeded();

        private readonly string _connectionString;

        static DapperSqlDb()
        {
            SqlMapper.AddTypeMap(typeof(DateTime), DbType.DateTime2);
        }

        public DapperSqlDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Result> Execute(string sql, object queryParams = null, bool errorIfMissing = false)
        {
            try
            {
                using (var c = OpenConnection())
                {
                    var result = await c.ExecuteAsync(sql, queryParams ?? new { });

                    if (errorIfMissing && result < 1)
                        return Result.Errored(ResultStatus.MissingResource, "Did not affect any records.");

                    return Result.Success(result);
                }
            }
            catch (Exception e)
            {
                return ToErrorResult(sql, e);
            }
        }

        public IDbConnection OpenConnection()
        {
            IDbConnection result = null;

            try
            {
                result = new SqlConnection(_connectionString);
                result.Open();
            }
            catch
            {
                result?.Dispose();
                throw;
            }

            return result;
        }

        public async Task<Result<IEnumerable<T>>> Query<T>(string sql, object queryParams = null)
        {
            try
            {
                using (var c = OpenConnection())
                {
                    var result = await c.QueryAsync<T>(sql, queryParams ?? new { });
                    return Result.Success(result);
                }
            }
            catch (Exception e)
            {
                return ToErrorResult<IEnumerable<T>>(sql, e);
            }
        }

        private Result<T> ToErrorResult<T>(string sql, Exception e) => ToErrorResult(sql, e).AsTypedError<T>();

        private Result ToErrorResult(string sql, Exception e)
        {
            var msg = $"Exception during SQL query/statement {e.Message}. SQL: {sql}";
            if (e.Message.Contains("conflict"))
                return Result.Errored(ResultStatus.Conflict, msg);
            if (e.Message.Contains("request limit") && e.Message.Contains("has been reached"))
                return Result.Errored(ResultStatus.DependencyFailure, msg);
            if (e.Message.Contains("The login failed."))
                return Result.Errored(ResultStatus.DependencyFailure, msg);
            if (e.Message.Contains("Connection Timeout Expired"))
                return Result.Errored(ResultStatus.DependencyFailure, msg);
            if (e.Message.Contains("retry the connection"))
                return Result.Errored(ResultStatus.DependencyFailure, msg);
            if (e.Message.Contains("transport-level error has occurred"))
                return Result.Errored(ResultStatus.DependencyFailure, msg);
            if (e.Message.Contains("all pooled connections were in use and max pool size was reached"))
                return Result.Errored(ResultStatus.ProcessingError, msg);
            if (e.Message.Contains("truncated"))
                return Result.InvalidRequest(msg);
            return Result.Errored(ResultStatus.DependencyFailure, msg);
        }
    }

    public static class DapperExtensions
    {
        public static DbString AsDbString(this string value) => value.AsDbString(value.Length);
        public static DbString AsDbString(this string value, int length) => new DbString {Value = value, IsAnsi = true, Length = length};

        public static async Task<Result<T>> QuerySingle<T>(this DapperSqlDb db, string sql, object queryParams = null)
        {
            return await db.Query<T>(sql, queryParams ?? new { })
                .Then(x => x.ToList())
                .Then(x =>
                    x.Count.Equals(1)
                        ? x.First()
                        : x.Count.Equals(0)
                            ? Result<T>.Errored(ResultStatus.MissingResource, $"No matching sql records found for {typeof(T).Name}")
                            : Result<T>.Errored(ResultStatus.ProcessingError, $"Expected exactly one sql record, but {x.Count} were returned."));
        }

        public static async Task<Result<T>> QueryFirst<T>(this DapperSqlDb db, string sql, object queryParams = null)
            => await QueryFirst<T>(db, sql, typeof(T).Name, queryParams);

        public static async Task<Result<T>> QueryFirst<T>(this DapperSqlDb db, string sql, string itemContext, object queryParams = null)
        {
            if (string.Equals(itemContext, "string", StringComparison.InvariantCultureIgnoreCase))
                throw new ArgumentException("Developer Error: Item Context must be supplied when returning a primitive value from SQL.");

            return await db.Query<T>(sql, queryParams ?? new { })
                .Then(x => x.ToList())
                .Then(x =>
                    x.Any()
                        ? x.First()
                        : Result<T>.Errored(ResultStatus.MissingResource, $"No matching sql records found for {itemContext}"));
        }

        public static async Task<Result> Execute(this DapperSqlDb db, string sql, DataTable dt, string tableValueParameterName, string tableValueParameterType)
            => await db.Execute(sql, new Dictionary<string, object> {{tableValueParameterName, dt.AsTableValuedParameter(tableValueParameterType)}});

        public static async Task<Result<IEnumerable<T>>> Query<T>(this DapperSqlDb db, string sql, DataTable dt, string tableValueParameterName, string tableValueParameterType)
            => await db.Query<T>(sql, new Dictionary<string, object> {{tableValueParameterName, dt.AsTableValuedParameter(tableValueParameterType)}});
    }
}
