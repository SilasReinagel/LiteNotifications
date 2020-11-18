using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Carvana;
using LiteNotifications.WebApi;
using LiteNotifications.WebApi.Auth;
using LiteNotifications.WebApi.Contracts;
using NUnit.Framework;

namespace LiteNotifications.Tests
{
    public class ApiTests
    {
        private static HttpClient _client = TestConfig.TestServer.Value;
        
        [Test]
        public async Task StatusController_Ok()
        {
            var resp = await _client.GetAsync("api/status");
            
            Assert.AreEqual(HttpStatusCode.OK, resp.StatusCode);
        }

        [Test]
        public async Task AuthController_Login()
        {
            var resp = await _client.PostAsync("api/auth/login", new StringContent(Json.Serialize(TestConfig.SampleLoginRequest), Encoding.UTF8, "application/json"));
            
            Assert.AreEqual(HttpStatusCode.OK, resp.StatusCode);
        }

        [Test]
        public async Task AddOutlet()
        {
            // TODO Replace with selected Login Group
            //var login = await GetPostResponseContent<LoginResponse>("api/auth/login", TestConfig.SampleLoginRequest);
            //var groups = login.Groups;

            var resp = await GetPostResponseContent<Unit>("api/manage/outlet",
                new AddOutletRequest {GroupId = 1, OutletType = "Email", Target = "silas.reinagel@gmail.com"});
            
            Assert.IsTrue(resp.Succeeded(), resp.ErrorMessage);
        }

        private async Task<Result<TResp>> GetPostResponseContent<TResp>(string url, object postBody)
        {
            var resp = await _client.PostAsync(url, new StringContent(Json.Serialize(postBody), Encoding.UTF8, "application/json"));
            var rawString = await resp.Content.ReadAsStringAsync();
            var respContent = Json.Deserialize<MutResult<TResp>>(rawString);
            return respContent.ToResult();
        }

        public class MutResult<T>
        {
            public T Content { get; set; }
            public string ErrorMessage { get; set; }
            public ResultStatus Status { get; set; }
            
            public Result<T> ToResult() => Status == ResultStatus.Succeeded ? new Result<T>(Content) : new Result<T>(Status, ErrorMessage);
        }
    }
}
