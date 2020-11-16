using System;
using System.Net.Http;
using LiteNotifications.WebApi;
using LiteNotifications.WebApi.Auth;
using LiteNotifications.WebApi.Infrastructure.Sql;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace LiteNotifications.Tests
{
    public static class TestConfig
    {
        public static LoginUserRequest SampleLoginRequest => new LoginUserRequest {Email = "admin@test.com", Password = "abc123"};
        public static DapperSqlDb SqlDb => new DapperSqlDb(new EnvironmentVariable("NotifyAppSqlConnection"));
        
        public static readonly Lazy<HttpClient> TestServer = new Lazy<HttpClient>(
            () => new TestServer(
                    new WebHostBuilder()
                        .UseStartup<Startup>())
                .CreateClient());
    }
}
