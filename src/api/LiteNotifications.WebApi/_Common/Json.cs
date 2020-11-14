using System.Text.Json;

namespace LiteNotifications.WebApi._Common
{
    public static class Json
    {
        public static string Serialize(object o) => JsonSerializer.Serialize(o);
        public static T Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json);
    }
}