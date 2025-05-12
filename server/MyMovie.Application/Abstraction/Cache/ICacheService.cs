namespace MyMovie.Application.Abstraction.Cache
{
    /// <summary>
    /// Interface for cache service.
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Get cache item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// Set cache item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetAsync<T>(string key, T value);

        /// <summary>
        /// Remove cache item.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task RemoveAsync(string key);
    }
}
