using FluentResults;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MyMovie.Application.Abstraction.Movies;

namespace MyMovie.SyncFunction
{
    /// <summary>
    /// Movie sync azure function
    /// </summary>
    /// <param name="loggerFactory"></param>
    /// <param name="providers"></param>
    /// <param name="movieService"></param>
    public class MovieSyncFunction(ILoggerFactory loggerFactory, IEnumerable<IMovieProvider> providers,
        IMovieService movieService)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<MovieSyncFunction>();
        private readonly IMovieService _movieService = movieService;

        /// <summary>
        /// Time triggered sync function 
        /// </summary>
        /// <param name="myTimer"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>

        [Function("Sync")]
        public async Task Run([TimerTrigger("%Functions.Sync.TimeTrigger%", RunOnStartup = true)] TimerInfo myTimer)
        {
            _logger.LogInformation($"{GetType().Name} executed at: {DateTime.Now}");

            if (providers == null || !providers.Any())
            {
                _logger.LogError($"{GetType().Name}: Movie providers are not available.");
                throw new ApplicationException($"{GetType().Name}: Movie providers are not available.");
            }

            foreach (var provider in providers)
            {
                try
                {
                    var result = await _movieService.SyncMoviesAsync(provider);
                    if (result.IsFailed)
                    {
                        _logger.LogError($"{GetType().Name}: Error {result.Errors.FirstOrDefault()?.Message}");
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"{GetType().Name}: Error {e.Message}");
                    throw;
                }
            }

            var deleteResult = await _movieService.DeleteOldMoviesAsync();
            if (deleteResult.IsFailed)
            {
                _logger.LogError($"{GetType().Name} :Deleting old movies from storage failed.");
            }

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
