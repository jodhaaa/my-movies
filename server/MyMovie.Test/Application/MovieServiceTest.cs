using FakeItEasy;
using FluentResults;
using Microsoft.Extensions.Logging;
using MyMovie.Application.Abstraction.Cache;
using MyMovie.Application.Abstraction.Data;
using MyMovie.Application.Abstraction.Movies;
using MyMovie.Application.Dto;
using MyMovie.Application.Dto.Movie;
using MyMovie.Application.Services;
using Shouldly;

namespace MyMovie.Test.Application
{
    /// <summary>
    /// unit testing for MovieService.
    /// </summary>
    public class MovieServiceTest
    {
        readonly IDataStore dataStore = A.Fake<IDataStore>();
        readonly ILoggerFactory loggerFactory = A.Fake<ILoggerFactory>();
        readonly ICacheService cacheService = A.Fake<ICacheService>();
        readonly MovieService movieService;
        public MovieServiceTest()
        {
            movieService = new MovieService(dataStore, loggerFactory, cacheService);
        }

        /// <summary>
        /// Test GetMoviesAsync.
        /// </summary>
        /// <param name="withCache"></param>
        /// <returns></returns>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Success_GetMoviesAsync(bool withCache)
        {
            var cachedData = new PagedResult<MergedMovieDto>()
            {
                TotalCount = 1,
                Items = new List<MergedMovieDto>()
                {
                    new MergedMovieDto()
                    {
                        Id = "Test",
                        Prices = new List<MergedPriceDto>()
                        {
                            new MergedPriceDto()
                            {
                                Provider = "Test",
                                Price = 12
                            }
                        }
                    }
                }
            };

            var offset = 1;
            var pageSize = 2;
            var sortBy = "Test";

            var cacheKey = $"GetMoviesAsync:{offset}:{pageSize}:{sortBy}";
            A.CallTo(() => cacheService.GetAsync<PagedResult<MergedMovieDto>?>(A<string>.Ignored)).Returns(Task.FromResult(withCache ? cachedData: null));
            A.CallTo(() => dataStore.GetMergedMoviesAsync(offset, pageSize, sortBy)).Returns(Task.FromResult(Result.Ok(cachedData)));

            var result = await movieService.GetMoviesAsync(offset, pageSize, sortBy);
            var cacheServiceCalls = Fake.GetCalls(cacheService);
            var dataStoreCalls = Fake.GetCalls(dataStore);

            cacheServiceCalls
                .Count(call => call.Method.Name == "GetAsync").ShouldBe(1);

            cacheServiceCalls
                .Count(call => call.Method.Name == "SetAsync").ShouldBe(withCache ? 0 :1);

            dataStoreCalls
                .Count(call => call.Method.Name == "GetMergedMoviesAsync").ShouldBe(withCache ? 0 : 1);

            result.IsSuccess.ShouldBe(true);
            result.Value.TotalCount.ShouldBe(1);
        }

        /// <summary>
        /// Test UpsertMovieAsync
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Success_UpsertMovieAsync()
        {
            var result = await movieService.UpsertMovieAsync(new MovieDto(), "Test");
            var dataStoreCalls = Fake.GetCalls(dataStore);

            dataStoreCalls
                .Count(call => call.Method.Name == "UpsertMovieAsync").ShouldBe(1);
        }

        /// <summary>
        /// Test DeleteOldMoviesAsync
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Success_DeleteOldMoviesAsync()
        {
            var result = await movieService.DeleteOldMoviesAsync();
            var dataStoreCalls = Fake.GetCalls(dataStore);

            dataStoreCalls
                .Count(call => call.Method.Name == "DeleteOldMoviesAsync").ShouldBe(1);
        }

        /// <summary>
        /// Test SyncMoviesAsync
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Success_SyncMoviesAsync()
        {
            var provider = A.Fake<IMovieProvider>();
            A.CallTo(() => provider.ProviderName).Returns("Test");

            var id1 = "test1";
            var name1 = "Test1";
            var movie1 = new MovieDto()
            {
                Id = id1,
                Title = name1
            };

            var id2 = "test2";
            var name2 = "Test2";
            var movie2 = new MovieDto()
            {
                Id = id2,
                Title = name2
            };

            var movieList = new List<MovieBaseDto>() { movie1, movie2 }.AsEnumerable();

            A.CallTo(() => provider.GetAsync()).Returns(Task.FromResult(Result.Ok(movieList)));
            A.CallTo(() => provider.GetByIdAsync(id1)).Returns(Task.FromResult(Result.Ok(movie1)));
            A.CallTo(() => provider.GetByIdAsync(id2)).Returns(Task.FromResult(Result.Ok(movie2)));

            var result = await movieService.SyncMoviesAsync(provider);

            var dataStoreCalls = Fake.GetCalls(dataStore);
            dataStoreCalls
                .Count(call => call.Method.Name == "UpsertMovieAsync").ShouldBe(2);
        }
    }
}