using System;
using System.Runtime.Caching;

namespace Web2.Utils
{
    public class Cache
    {
        private static readonly MemoryCache cache = MemoryCache.Default;

        public static object GetCache(string key)
        {
            return cache.Get(key);
        }

        public static void SetCache(string key, object objeto, int tempoSegundos)
        {
            cache.Set(key, objeto, DateTimeOffset.Now.AddSeconds(tempoSegundos));
        }

        public static void ClearCache(string key)
        {
            cache.Remove(key);
        }
    }
}