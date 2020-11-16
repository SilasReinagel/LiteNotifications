using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LiteNotifications.WebApi;
using NUnit.Framework;

namespace LiteNotifications.Tests
{
    public class ApiTests
    {
        [Test]
        public async Task StatusController_Ok()
        {
            var resp = await TestConfig.TestServer.Value.GetAsync("api/status");
            
            Assert.AreEqual(HttpStatusCode.OK, resp.StatusCode);
        }

        [Test]
        public async Task AuthController_Login()
        {
            var resp = await TestConfig.TestServer.Value.PostAsync("api/auth/login", new StringContent(Json.Serialize(TestConfig.SampleLoginRequest), Encoding.UTF8, "application/json"));
            
            Assert.AreEqual(HttpStatusCode.OK, resp.StatusCode);
        }
    }
}
