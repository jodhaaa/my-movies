using MyMovie.Application.Dto.Movie;

namespace MyMovie.Application.Dto
{
    /// <summary>
    /// Page results
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// Items list.
        /// </summary>
        public IEnumerable<T> Items { get; set; }
        
        /// <summary>
        /// Total items in the store.
        /// </summary>
        public int TotalCount { get; set; }
    }
}