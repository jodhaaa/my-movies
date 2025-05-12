using Microsoft.Extensions.Logging;
using MyMovie.Application.Abstraction.Movies;
using MyMovie.Application.Dto.Config;
using MyMovie.Application.Dto.Movie;
using Newtonsoft.Json;
using System.Net;
using FluentResults;

namespace MyMovie.Infrastructure.Movies.Providers
{
    /// <summary>
    /// Base class for concrete movie providers.
    /// </summary>
    public abstract class BaseMovieProvider : IMovieProvider
    {
        public abstract string ProviderName { get; }

        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly Provider _provider;

        protected BaseMovieProvider(Provider[] providers, IHttpClientFactory httpClientFactor, ILoggerFactory loggerFactory)
        {
            _httpClient = httpClientFactor.CreateClient(ProviderName);
            _logger = loggerFactory.CreateLogger<BaseMovieProvider>();
            _provider = providers.First(p => p.Name == ProviderName);
        }

        /// <summary>
        /// Get movie list.
        /// </summary>
        /// <returns></returns>
        public async Task<Result<IEnumerable<MovieBaseDto>>> GetAsync()
        {
            _logger.LogInformation($"{GetType().Name} start getting movie meta data.");

            var result = await FetchDataAsync(_provider.MovieListUrl);

            if (result.IsFailed)
            {
                var message = result.Errors.FirstOrDefault()?.Message;
                _logger.LogError($"{GetType().Name} error fetching data: {message}");
                return Result.Fail(message);
            }

            var movies = JsonConvert.DeserializeObject<ApiData>(result.Value);
            _logger.LogInformation($"{GetType().Name} end getting movie meta data.");

            return Result.Ok(movies.Movies);
        }

        /// <summary>
        /// Get movie by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<MovieDto>> GetByIdAsync(string id)
        {
            _logger.LogInformation($"{GetType().Name} start getting movie meta data.");

            var result = await FetchDataAsync(string.Format(_provider.MovieUrl, id));

            if (result.IsFailed)
            {
                var message = result.Errors.FirstOrDefault()?.Message;
                _logger.LogError($"{GetType().Name} error fetching data: {message}");
                return Result.Fail(message);
            }

            var movie = JsonConvert.DeserializeObject<MovieDto>(result.Value);
            _logger.LogInformation($"{GetType().Name} end getting movie meta data.");

            return Result.Ok(movie);
        }

        /// <summary>
        /// Fetch data from movie api
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <returns></returns>
        protected async Task<Result<string>> FetchDataAsync(string apiUrl)
        {
            try
            {
                HttpResponseMessage responseMessage = await _httpClient.GetAsync(apiUrl);
                var message = await responseMessage.Content.ReadAsStringAsync();

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    // Api Data
                    return Result.Ok(message);
                }
                else
                {
                    _logger.LogError($"Error: {GetType().Name} FetchData api call failed");
                    throw new WebException(message);
                }
            }
            catch (WebException ex)
            {
                _logger.LogError("Error: WebException: " + ex.Message);
                return Result.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
                return Result.Fail(ex.Message);
            }
        }
    }
}
