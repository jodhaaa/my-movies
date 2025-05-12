using FluentResults;
using MyMovie.Application.Dto;
using MyMovie.Application.Dto.Movie;

namespace MyMovie.Application.Abstraction.Data
{
    /// <summary>
    /// Interface for data services
    /// </summary>
    public interface IDataStore
    {
        /// <summary>
        /// Get merged movies
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        Task<Result<PagedResult<MergedMovieDto>>> GetMergedMoviesAsync(int offset, int pageSize, string sortBy);

        /// <summary>
        /// Upsert movies.
        /// </summary>
        /// <param name="movie"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        Task<Result> UpsertMovieAsync(MovieDto movie, string provider);

        /// <summary>
        /// Delete old movies
        /// </summary>
        /// <returns></returns>
        Task<Result> DeleteOldMoviesAsync();

        /// <summary>
        /// Create the schema.
        /// </summary>
        /// <returns></returns>
        Result CreateSchema();
    }
}
