using System;
using System.Text;

namespace LiteNotifications.WebApi._Common
{
    public static class StringExtensions
    {
        public static string Required(this string s, string name)
        {
            if (string.IsNullOrWhiteSpace(s))
                throw new ArgumentNullException($"String '{name}' was null or empty.");
            return s;
        }
        
        public static byte[] FromWebSafeBase64(this string input)
        {
            var postfix = "==".Substring(0, (3 * input.Length) % 4);
            var base64 = input
                             .Replace('-', '+')
                             .Replace('_', '/')
                         + postfix;
            return Convert.FromBase64String(base64);
        }
        
        public static string ToWebSafeBase64(this string input)
        {
            return ToWebSafeBase64(Encoding.UTF8.GetBytes(input));
        }
        
        public static string ToWebSafeBase64(this byte[] input)
        {
            return Convert.ToBase64String(input)
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');
        }
    }
}
