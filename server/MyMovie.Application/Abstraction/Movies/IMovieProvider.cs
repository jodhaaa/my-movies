using FluentResults;
using MyMovie.Application.Dto.Movie;

namespace MyMovie.Application.Abstraction.Movies
{
    /// <summary>
    /// Interface for movie provider.
    /// </summary>
    public interface IMovieProvider
    {
        /// <summary>
        /// Provider name.
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// Get movies
        /// </summary>
        /// <returns></returns>
        Task<Result<IEnumerable<MovieBaseDto>>> GetAsync();

        /// <summary>
        /// Get movie by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Result<MovieDto>> GetByIdAsync(string id);
    }
}
