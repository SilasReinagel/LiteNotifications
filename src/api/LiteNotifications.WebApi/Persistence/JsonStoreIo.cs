using System.IO;
using System.Net.Http;
using System.Text;

namespace LiteNotifications.WebApi.Persistence
{
    public sealed class JsonStoreIo : SimpleIo
    {
        private const string BaseUrl = "https://www.jsonstore.io/";
        private readonly HttpClient _client = new HttpClient();
        
        public void Put<T>(string name, T data)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, Url(name));
            var rawPostJson = Json.Serialize(data);
            req.Content = new StringContent(rawPostJson, Encoding.UTF8, "application/json");
            var resp = _client.SendAsync(req).ConfigureAwait(false).GetAwaiter().GetResult();
            if (!resp.IsSuccessStatusCode)
                throw new IOException($"Unable to Put {Url(name)}");
        }

        public T Get<T>(string name)
        {            
            var req = new HttpRequestMessage(HttpMethod.Get, Url(name));
            var resp = _client.SendAsync(req).ConfigureAwait(false).GetAwaiter().GetResult();
            if (!resp.IsSuccessStatusCode)
                throw new IOException($"Unable to Get {Url(name)}");
            var rawJson = resp.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = Json.Deserialize<JsonStoreResult<T>>(rawJson);
            return result.result;
        }

        public bool Contains(string name)
        {
            var obj = Get<object>(name);
            return obj != null;
        }

        public void Delete(string name)
        {
            var req = new HttpRequestMessage(HttpMethod.Delete, Url(name));
            var resp = _client.SendAsync(req).ConfigureAwait(false).GetAwaiter().GetResult();
            if (!resp.IsSuccessStatusCode)
                throw new IOException($"Unable to Delete {Url(name)}");
        }

        private string Url(string name) => Path.Combine(BaseUrl, name);

        public class JsonStoreResult<T>
        {
            public bool ok { get; set; }
            public T result { get; set; }
        }
    }
}
