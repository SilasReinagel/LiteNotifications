using System;
using System.Security.Cryptography;
using System.Text;

namespace LiteNotifications.WebApi
{
    public sealed class Jwt
    {
        public static string CreateToken(string secret, string userId, string email) 
            => Signed(secret, Payload(Header(), Claims(userId, email, GetExpirationTime())));

        private static string GetExpirationTime() 
            => DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds().ToString();

        private static string Header() 
            => "{\"typ\":\"JWT\",\"alg\":\"HS256\"}";

        private static string Claims(string userId, string username, string expiresAtUtc) 
            => $"{{\"sub\":\"{userId}\", \"exp\": {expiresAtUtc}, \"username\": \"{username}\"}}";

        private static string Payload(string headerJson, string claimsJson) 
            => Base64UrlEncode(headerJson) + "." + Base64UrlEncode(claimsJson);

        private static string Signed(string key, string payload) 
            => payload + "." + Base64UrlEncode(Hmac256(Bytes(key), Bytes(payload)));

        private static byte[] Bytes(string src) => Encoding.UTF8.GetBytes(src);
        private static string Utf8(byte[] bytes) => Encoding.UTF8.GetString(bytes);
        private static string Base64UrlEncode(string src) => Base64UrlEncode(Bytes(src));
        private static string Base64UrlEncode(byte[] bytes) =>
            Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");

        private static byte[] Hmac256(byte[] key, byte[] message)
        {
            var hmac = new HMACSHA256(key);
            var hash = hmac.ComputeHash(message);
            return hash;
        } 
    }
}
