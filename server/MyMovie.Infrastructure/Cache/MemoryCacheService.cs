using Microsoft.Extensions.Configuration;
using MyMovie.Application.Abstraction.Cache;
using System.Runtime.Caching;

namespace MyMovie.Infrastructure.Cache
{
    /// <summary>
    /// Memory cache service.
    /// </summary>
    /// <param name="config"></param>
    public class MemoryCacheService(IConfiguration config) : ICacheService
    {
        private readonly ObjectCache _cache = MemoryCache.Default;
        private readonly TimeSpan _expiration = TimeSpan.FromMinutes((int.Parse(config["CacheInvalidation"])));

        /// <summary>
        /// Get item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key)
        {
            return _cache.Contains(key) ? (T)_cache[key] : default;
        }

        /// <summary>
        /// Set item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SetAsync<T>(string key, T value)
        {
            _cache.Set(key, value, DateTimeOffset.Now.Add(_expiration));
        }

        /// <summary>
        /// Remove item.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task RemoveAsync(string key)
        {
            _cache.Remove(key);
        }
    }
}
