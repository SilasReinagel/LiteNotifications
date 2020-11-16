using System.Collections.Generic;

namespace LiteNotifications.WebApi
{
    public static class EnumerableExtensions
    {
        public static Dictionary<TKey, TValue> With<TKey, TValue>(this Dictionary<TKey, TValue> d, TKey key, TValue value)
        {
            d[key] = value;
            return d;
        }
    }
}
