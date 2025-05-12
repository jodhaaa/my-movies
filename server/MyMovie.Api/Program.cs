using MyMovie.Application.Abstraction.Cache;
using MyMovie.Application.Abstraction.Data;
using MyMovie.Application.Abstraction.Movies;
using MyMovie.Application.Services;
using MyMovie.Infrastructure.Cache;
using MyMovie.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)

    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddSingleton<IDataStore>(s =>
{
    var loggerFactory = s.GetService<ILoggerFactory>();
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
    var datastore = new SqliteDataStore(connectionString, loggerFactory, builder.Configuration);
    datastore.CreateSchema();
    return datastore;
});

builder.Services.AddSingleton<ICacheService, MemoryCacheService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// No pagination, fetch all data for the moment
app.MapGet("/movies", async (int offset, int pageSize, string sortBy, IMovieService movieService) =>
    {
        var result = await movieService.GetMoviesAsync(offset, pageSize, sortBy);
        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }
        else
        {
            return Results.Problem("Error loading movies");
        }
    })
    .WithName("GetMovies")
    .WithOpenApi();

app.Run();