using System.Collections;
using System.Text.Json;

namespace LiteNotifications.WebApi
{
    public static class Json
    {
        public static string Serialize(object o) => JsonSerializer.Serialize(o);
        public static T Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
    }
}
