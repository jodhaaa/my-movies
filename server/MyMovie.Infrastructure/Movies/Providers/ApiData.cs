using MyMovie.Application.Dto.Movie;

namespace MyMovie.Infrastructure.Movies.Providers
{
    /// <summary>
    /// Api data from providers
    /// </summary>
    internal class ApiData
    {
        public IEnumerable<MovieBaseDto> Movies { get; set; }
    }
}
