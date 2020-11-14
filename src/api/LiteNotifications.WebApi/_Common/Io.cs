using System;

namespace LiteNotifications.WebApi._Common
{
    public interface Io
    {
        void Put<T>(string name, T data);
        T Get<T>(string name);
        bool Contains(string name);
        void Delete(string name);
    }
    
    public static class IoExtensions
    {
        public static T GetInitialized<T>(this Io io, string name, Func<T> createDefault)
        {
            if (!io.Contains(name))
                io.Put(name, createDefault());
            var result = io.Get<T>(name);
            return result == null ? createDefault() : result;
        }
    }
}
