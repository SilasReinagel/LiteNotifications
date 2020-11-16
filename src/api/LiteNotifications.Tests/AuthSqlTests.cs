using System.Threading.Tasks;
using LiteNotifications.WebApi.Auth;
using LiteNotifications.WebApi.Controllers;
using NUnit.Framework;

namespace LiteNotifications.Tests
{
    public sealed class AuthSqlTests
    {
        private readonly AuthSql _auth = new AuthSql("secret", TestConfig.SqlDb);

        [Test, Ignore("One-Time Only")]
        public async Task Auth_CreateUser_Succeeded()
        {
            var resp = await _auth.Get(new RegisterUserRequest { Email = "admin@test.com", Password = "abc123" });
            
            Assert.IsTrue(resp.Succeeded(), resp.ErrorMessage);
        }

        [Test]
        public async Task Auth_Login_Succeeded()
        {
            var resp = await _auth.Get(TestConfig.SampleLoginRequest);
            
            Assert.IsTrue(resp.Succeeded(), resp.ErrorMessage);
        }
    }
}
