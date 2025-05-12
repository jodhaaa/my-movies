using Microsoft.Extensions.Logging;
using MyMovie.Application.Dto.Config;

namespace MyMovie.Infrastructure.Movies.Providers
{
    /// <summary>
    /// Movie Provider for CinemaWorld
    /// </summary>
    /// <param name="providers"></param>
    /// <param name="httpClientFactor"></param>
    /// <param name="loggerFactory"></param>
    public class CinemaWorldProvider(Provider[] providers, IHttpClientFactory httpClientFactor,
        ILoggerFactory loggerFactory) : BaseMovieProvider(providers, httpClientFactor, loggerFactory)
    {
        public override string ProviderName => Application.Const.Providers.CinemaWorld;
    }
}
