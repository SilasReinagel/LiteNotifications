using System;
using System.Net.Http;
using LiteNotifications.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace LiteNotifications.Tests
{
    public static class TestConfig
    {
        public static readonly Lazy<HttpClient> TestServer = new Lazy<HttpClient>(
            () => new TestServer(
                    new WebHostBuilder()
                        .UseStartup<Startup>())
                .CreateClient());
    }
}
