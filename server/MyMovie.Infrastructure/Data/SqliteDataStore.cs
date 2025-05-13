using Dapper;
using FluentResults;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyMovie.Application.Abstraction.Data;
using MyMovie.Application.Dto;
using MyMovie.Application.Dto.Movie;
using MyMovie.Infrastructure.Data.Entities;

namespace MyMovie.Infrastructure.Data
{
    /// <summary>
    /// Sqlite data store.
    /// </summary>
    public class SqliteDataStore : IDataStore
    {
        protected bool _inMemory = false;
        protected readonly string _connectionString;
        private readonly ILogger _logger;
        private readonly int _dbInvalidation;

        public SqliteDataStore(string connectionString, ILoggerFactory loggerFactory, IConfiguration config)
        {
            _connectionString = connectionString;
            _logger = loggerFactory.CreateLogger<SqliteDataStore>();
            int.TryParse(config["DBInvalidation"], out _dbInvalidation);
        }

        /// <summary>
        /// Get paged merged movies.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        public async Task<Result<PagedResult<MergedMovieDto>>> GetMergedMoviesAsync(int offset, int pageSize, string sortBy)
        {
            return await ExecuteSql(async (SqliteConnection connection) =>
            {
                try
                {
                    // TODO: Move to config
                    var validTime = DateTime.UtcNow.AddMinutes(-(_dbInvalidation));

                    switch (sortBy)
                    {
                        case "Title DESC":
                        case "Title ASC":
                        case "Year DESC":
                        case "Year ASC":
                            break;
                        default:
                            sortBy = "Title ASC";
                            break;
                    }

                    // This logic is for remove old data. Eg:- not updated for last 2 hours. 
                    var movies = await connection.QueryAsync<Movie>($"SELECT * FROM Movies WHERE LastModifiedDate > @ValidTime ORDER BY {sortBy} LIMIT @PageSize OFFSET @Offset;",
                        new { ValidTime = validTime, PageSize = pageSize, Offset = offset });


                    var movieDtos = movies.Select(m => m.ToDto());

                    var totalMovies = await connection.QueryFirstAsync<int>(
                        "SELECT Count(*) AS TotalRecords FROM Movies WHERE LastModifiedDate > @ValidTime",
                        new { ValidTime = validTime });
                    return Result.Ok(new PagedResult<MergedMovieDto>() { Items = movieDtos, TotalCount = totalMovies });
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"{GetType().Name}:GetMoviesAsync failed.");
                    return Result.Fail(e.Message);
                }
            });
        }

        /// <summary>
        /// Delete old movies.
        /// </summary>
        /// <returns></returns>
        public async Task<Result> DeleteOldMoviesAsync()
        {

            return await ExecuteSql(async (SqliteConnection connection) =>
            {
                try
                {
                    var validTime = DateTime.UtcNow.AddMinutes(-(_dbInvalidation));
                    var rowsAffected = await connection.ExecuteAsync(
                            @$"DELETE FROM Movies WHERE LastModifiedDate <= @ValidTime;"
                        ,
                            new
                            {
                                ValidTime = validTime
                            });

                    return Result.Ok();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Failed to delete old movies.");
                    return Result.Fail($"Failed to delete old movies.");
                }
            });
        }

        /// <summary>
        /// Upsert movie.
        /// </summary>
        /// <param name="movie"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public async Task<Result> UpsertMovieAsync(MovieDto movie, string provider)
        {

            return await ExecuteSql(async (SqliteConnection connection) =>
            {
                var id = $"{movie.Type}:{movie.Title}:{movie.Year}";
                try
                {
                    var updatedTime = DateTime.UtcNow;
                    var rowsAffected = await connection.ExecuteAsync(
                            @$"INSERT INTO Movies
                            (Id, Title, Year, Poster, Type, Rated, Released, Runtime, Genre, Director, 
                            Writer, Actors, Plot, Language, Country, Awards, Metascore, Rating, Votes, {provider}Price,
                            CreatedDate, LastModifiedDate)
                         VALUES 
                            (@Id, @Title, @Year, @Poster, @Type, @Rated, @Released, @Runtime, @Genre, @Director, 
                            @Writer, @Actors, @Plot, @Language, @Country, @Awards, @Metascore, @Rating, @Votes , @Price,
                            @CreatedDate, @LastModifiedDate) ON CONFLICT(Id) DO 
                        UPDATE SET
                            Title = MAX(Title, excluded.Title),
                            Year = MAX(Year, excluded.Year),
                            Poster = CASE 
                                        WHEN excluded.Poster = '' THEN Poster
                                        ELSE excluded.Poster
                                    END,
                            Type = MAX(Type, excluded.Type),
                            Rated = MAX(Rated, excluded.Rated),
                            Released = MAX(Released, excluded.Released),
                            Runtime = MAX(Runtime, excluded.Runtime),
                            Genre = MAX(Genre, excluded.Genre),
                            Director = MAX(Director, excluded.Director),
                            Writer = MAX(Writer, excluded.Writer),
                            Actors = MAX(Actors,excluded.Actors),
                            Plot = MAX(Plot, excluded.Plot),
                            Language = MAX(Language, excluded.Language),
                            Country = MAX(Country, excluded.Country),
                            Awards = MAX(Awards, excluded.Awards),
                            Metascore = MAX(Metascore, excluded.Metascore),
                            Rating = MAX(Rating, excluded.Rating),
                            Votes = MAX(Votes, excluded.Votes),
                            {provider}Price = excluded.{provider}Price,
                            LastModifiedDate = excluded.LastModifiedDate;"
                        ,
                            new
                            {
                                Id = id,
                                Title = movie.Title,
                                Year = movie.Year,
                                Poster = movie.Poster,
                                Type = movie.Type,
                                Rated = movie.Rated,
                                Released = movie.Released,
                                Runtime = movie.Runtime,
                                Genre = movie.Genre,
                                Director = movie.Director,
                                Writer = movie.Writer,
                                Actors = movie.Actors,
                                Plot = movie.Plot,
                                Language = movie.Language,
                                Country = movie.Country,
                                Awards = movie.Awards,
                                Metascore = movie.Metascore,
                                Rating = movie.Rating,
                                Votes = movie.Votes,
                                Price = movie.Price,
                                CreatedDate = updatedTime,
                                LastModifiedDate = updatedTime,
                            });

                    if (rowsAffected == 0)
                    {
                        _logger.LogError($"Failed to upsert Movie with Id: {id}.");
                        return Result.Fail($"Failed to upsert Movie with Id: {id}.");
                    }

                    return Result.Ok();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"Failed to upsert Movie with Id: {id}.");
                    return Result.Fail($"Failed to upsert Movie with Id: {id}.");
                }
            });
        }

        /// <summary>
        /// Execute in sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        protected virtual async Task<T> ExecuteSql<T>(Func<SqliteConnection, Task<T>> action)
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();
            return await action(connection);
        }

        public Result CreateSchema()
        {
            try
            {
                string createMovieTableQuery = @"
                CREATE TABLE IF NOT EXISTS Movies (
                    Id TEXT PRIMARY KEY NOT NULL,
                    Title TEXT NOT NULL,
                    Year TEXT NOT NULL,
                    Poster TEXT NULL,
                    Type TEXT NULL,
                    Rated TEXT NULL,
                    Released TEXT NULL,
                    Runtime TEXT NULL,
                    Genre TEXT NULL,
                    Director TEXT NULL,
                    Writer TEXT NULL,
                    Actors TEXT NULL,
                    Plot TEXT NULL,
                    Language TEXT NULL,
                    Country TEXT NULL,
                    Awards TEXT NULL,
                    Metascore TEXT NULL,
                    Rating TEXT NULL,
                    Votes TEXT NULL,
                    CinemaWorldPrice REAL NULL,
                    FilmWorldPrice REAL NULL,
                    CreatedDate TEXT NOT NULL,
                    LastModifiedDate TEXT NOT NULL
                );";

                if (!_inMemory)
                {
                    var dbFilePath = ExtractFilePathFromConnectionString(_connectionString);

                    //Return is file exists
                    if (File.Exists(dbFilePath))
                        return Result.Ok();

                    // Create the directory if it doesn't exist
                    var directory = Path.GetDirectoryName(dbFilePath);
                    if (directory != null && !Directory.Exists(directory))
                        Directory.CreateDirectory(directory);
                }

                ExecuteSql(async (SqliteConnection connection) =>
                {
                    await connection.ExecuteAsync(createMovieTableQuery);
                    return Task.CompletedTask;
                }).Wait();

                return Result.Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{GetType().Name}:CreateSchema failed.");
                return Result.Fail(e.Message);
            }
        }

        /// <summary>
        /// Get file path from connection string.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private string ExtractFilePathFromConnectionString(string connectionString)
        {
            var builder = new System.Data.Common.DbConnectionStringBuilder
            {
                ConnectionString = connectionString
            };

            if (builder.TryGetValue("Data Source", out var dataSource))
            {
                return dataSource.ToString()!;
            }

            throw new ArgumentException("The connection string does not contain a 'Data Source' key.", nameof(connectionString));
        }
    }
}
