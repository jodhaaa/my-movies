using Microsoft.Extensions.Logging;
using MyMovie.Application.Dto.Config;

namespace MyMovie.Infrastructure.Movies.Providers
{
    /// <summary>
    /// Movie Provider for FilmWorld
    /// </summary>
    /// <param name="providers"></param>
    /// <param name="httpClientFactor"></param>
    /// <param name="loggerFactory"></param>
    public class FilmWorldProvider(Provider[] providers, IHttpClientFactory httpClientFactor,
        ILoggerFactory loggerFactory) : BaseMovieProvider(providers, httpClientFactor, loggerFactory)
    {
        public override string ProviderName => Application.Const.Providers.FilmWorld;
    }
}
