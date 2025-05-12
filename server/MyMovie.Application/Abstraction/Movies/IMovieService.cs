using FluentResults;
using MyMovie.Application.Dto;
using MyMovie.Application.Dto.Movie;

namespace MyMovie.Application.Abstraction.Movies
{
    /// <summary>
    /// Interface for movie service layer.
    /// </summary>
    public interface IMovieService
    {
        /// <summary>
        /// Get paged movies
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        Task<Result<PagedResult<MergedMovieDto>>> GetMoviesAsync(int offset, int pageSize, string sortBy);

        /// <summary>
        /// Upsert movie.
        /// </summary>
        /// <param name="movie"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        Task<Result> UpsertMovieAsync(MovieDto movie, string provider);

        /// <summary>
        /// Sync movies.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        Task<Result> SyncMoviesAsync(IMovieProvider provider);

        /// <summary>
        /// Delete old movies.
        /// </summary>
        /// <returns></returns>
        Task<Result> DeleteOldMoviesAsync();
    }
}
