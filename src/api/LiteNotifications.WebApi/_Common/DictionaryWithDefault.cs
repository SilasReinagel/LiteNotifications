using System;
using System.Collections.Generic;

namespace LiteNotifications.WebApi._Common
{
    public class DictionaryWithDefault<TKey, TValue> : Dictionary<TKey, TValue>
    {
        private readonly TValue _defaultValue;
        
        private static IEqualityComparer<TKey> GetComparer()
        {
            if (typeof(TKey) == typeof(string)) return (IEqualityComparer<TKey>)(object)StringComparer.InvariantCultureIgnoreCase;
            return EqualityComparer<TKey>.Default;
        }

        public DictionaryWithDefault(TValue defaultValue): base(GetComparer())
        {
            _defaultValue = defaultValue;
        }

        public new TValue this[TKey key]
        {
            get => ContainsKey(key) ? base[key] : _defaultValue;
            set => base[key] = value;
        }
    }
}
