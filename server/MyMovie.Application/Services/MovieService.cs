using FluentResults;
using Microsoft.Extensions.Logging;
using MyMovie.Application.Abstraction.Cache;
using MyMovie.Application.Abstraction.Data;
using MyMovie.Application.Abstraction.Movies;
using MyMovie.Application.Dto;
using MyMovie.Application.Dto.Movie;

namespace MyMovie.Application.Services
{
    /// <summary>
    /// Concrete Movie Service.
    /// </summary>
    /// <param name="dataStore"></param>
    /// <param name="loggerFactory"></param>
    /// <param name="cacheService"></param>
    public class MovieService(IDataStore dataStore, ILoggerFactory loggerFactory, ICacheService cacheService) : IMovieService
    {
        private readonly IDataStore _dataStore = dataStore;
        private readonly ICacheService _cacheService = cacheService;
        private readonly ILogger _logger = loggerFactory.CreateLogger<MovieService>();

        /// <summary>
        /// Get paged movies.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        public async Task<Result<PagedResult<MergedMovieDto>>> GetMoviesAsync(int offset, int pageSize, string sortBy)
        {
            try
            {
                var cacheKey = $"GetMoviesAsync:{offset}:{pageSize}:{sortBy}";
                var cacheObj = await _cacheService.GetAsync<PagedResult<MergedMovieDto>?>(cacheKey);
                if (cacheObj != null)
                {
                    return Result.Ok(cacheObj);
                }

                var result = await _dataStore.GetMergedMoviesAsync(offset, pageSize, sortBy);
                if (result.IsSuccess)
                {
                    await _cacheService.SetAsync<PagedResult<MergedMovieDto>?>(cacheKey, result.Value);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: GetMoviesAsync");
                return Result.Fail("Error: GetMoviesAsync");
            }
        }

        /// <summary>
        /// Upsert movie.
        /// </summary>
        /// <param name="movie"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public async Task<Result> UpsertMovieAsync(MovieDto movie, string provider)
        {
            try
            {
                return await _dataStore.UpsertMovieAsync(movie, provider);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: GetMovieByIdAsync");
                return Result.Fail("Error: GetMovieByIdAsync");
            }
        }

        /// <summary>
        /// Delete old movies.
        /// </summary>
        /// <returns></returns>
        public async Task<Result> DeleteOldMoviesAsync()
        {
            try
            {
                return await _dataStore.DeleteOldMoviesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: DeleteOldMoviesAsync");
                return Result.Fail("Error: DeleteOldMoviesAsync");
            }
        }

        /// <summary>
        /// Sync movies.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public async Task<Result> SyncMoviesAsync(IMovieProvider provider)
        {
            try
            {
                var getResult = await provider.GetAsync();
                if (getResult.IsFailed)
                {
                    _logger.LogError("Error fetching data: SyncMoviesAsync: GetAsync");
                    return Result.Fail("Error: SyncMoviesAsync");

                }

                foreach (var baseMovie in getResult.Value)
                {
                    var getIdResult = await provider.GetByIdAsync(baseMovie.Id);
                    if (getIdResult.IsFailed)
                    {
                        _logger.LogError("Error fetching data: SyncMoviesAsync: GetByIdAsync");
                       continue;
                    }

                    var result = await UpsertMovieAsync(getIdResult.Value, provider.ProviderName);
                    if (result.IsFailed)
                    {
                        _logger.LogError(
                            $"{GetType().Name}: SyncMoviesAsync :Saving movie id {baseMovie.Id}  to storage failed.");
                    }
                }

                return Result.Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: SyncMoviesAsync");
                return Result.Fail("Error: SyncMoviesAsync");
            }
        }
    }
}
