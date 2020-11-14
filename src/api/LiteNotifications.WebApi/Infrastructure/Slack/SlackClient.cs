using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LiteNotifications.WebApi._Common;

namespace LiteNotifications.WebApi.Infrastructure.Slack
{
    public class SlackClient
    {
        const string BaseUrl = "https://slack.com/api";
        readonly string _token;
        readonly HttpClient _httpClient = new HttpClient();

        public SlackClient(string token)
        {
            _token = token;
        }

        public async Task<TResponse> Post<TResponse>(string path, object queryParams = null)
        {
            var uri = GenerateUri(path, queryParams);
            var response = await _httpClient.PostAsync(uri, null);

            response.EnsureSuccessStatusCode();

            using (var content = response.Content)
            {
                var body = await content.ReadAsStringAsync();
                var result = Json.Deserialize<TResponse>(body);
                return result;
            }
        }

        public async Task<T> Get<T>(string path, object validParams = null)
        {
            string uri = GenerateUri(path, validParams);
            var response = await _httpClient.GetAsync(uri);

            response.EnsureSuccessStatusCode();

            using (var content = response.Content)
            {
                var body = await content.ReadAsStringAsync();
                var result = Json.Deserialize<T>(body);
                return result;
            }
        }

        string GenerateUri(string path, object validParams)
        {
            var cleanPath = path.Trim('/');
            var cleanParams = validParams ?? new { };

            var uri = $"{BaseUrl}/{cleanPath}?{GetQueryString(cleanParams)}";
            return uri;
        }

        string GetQueryString(object obj)
        {
            var props = obj.GetType()
                .GetProperties()
                .Where(w => w.GetValue(obj, null) != null)
                .Select(s => $"{s.Name}={WebUtility.UrlEncode(s.GetValue(obj, null).ToString())}")
                .ToList();
            props.Add($"token={_token}");

            return string.Join("&", props);
        }
    }
}
