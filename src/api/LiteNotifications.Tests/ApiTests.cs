using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;

namespace LiteNotifications.Tests
{
    public class ApiTests
    {
        [Test]
        public async Task StatusController_Ok()
        {
            var resp = await TestConfig.TestServer.Value.GetAsync("/status");
            
            Assert.AreEqual(HttpStatusCode.OK, resp.StatusCode);
        }
    }
}
