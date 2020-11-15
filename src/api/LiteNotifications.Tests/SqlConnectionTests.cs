using System;
using System.Threading.Tasks;
using LiteNotifications.WebApi._Common;
using LiteNotifications.WebApi.Infrastructure.Sql;
using NUnit.Framework;

namespace LiteNotifications.Tests
{
    public sealed class SqlConnectionTests
    {
         [Test]
         public async Task TestConnection()
         {
             var envVarName = "NotifyAppSqlConnection";
             if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(envVarName)))
                 Assert.Inconclusive("Required Environment Variable is missing.");
             
             var resp = await new DapperSqlDb(new EnvironmentVariable(envVarName)).QuerySingle<int>("SELECT 1");
             
             Assert.IsTrue(resp.Succeeded(), resp.ErrorMessage);
         }
    }
}
