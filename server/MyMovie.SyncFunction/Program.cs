using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyMovie.Application.Abstraction.Cache;
using MyMovie.Application.Abstraction.Data;
using MyMovie.Application.Abstraction.KeyVault;
using MyMovie.Application.Abstraction.Movies;
using MyMovie.Application.Dto.Config;
using MyMovie.Application.Services;
using MyMovie.Infrastructure.Cache;
using MyMovie.Infrastructure.Data;
using MyMovie.Infrastructure.KeyVault;
using MyMovie.Infrastructure.Movies.Providers;
using Polly;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration(builder =>
    {
        builder.AddJsonFile("appsettings.json");
        builder.AddUserSecrets<Program>();
    })
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddScoped<IMovieProvider, CinemaWorldProvider>();
        services.AddScoped<IMovieProvider, FilmWorldProvider>();

        services.AddScoped<IMovieService, MovieService>();

        // TODO: Replace local KeyVaultProvider
        // IMPORTANT: This is for local demonstration only.
        services.AddScoped<IKeyVaultProvider, KeyVaultProvider>();

        services.AddSingleton<ICacheService, MemoryCacheService>();

        services.AddSingleton<IDataStore>(s =>
        {
            var loggerFactory = s.GetService<ILoggerFactory>();
            string connectionString = configuration.GetConnectionString("DefaultConnection")!;
            var datastore = new SqliteDataStore(connectionString, loggerFactory, configuration);
            var result = datastore.CreateSchema();
            if (result.IsFailed)
            {
                throw new ApplicationException("DB schema creation failed");
            }

            return datastore;
        });

        var providers = configuration.GetSection("Providers").Get<Provider[]>();
        services.AddSingleton(providers);
        foreach (var provider in providers)
        {
            services.AddHttpClient(provider.Name, async (serviceProvider, client) =>
            {
                var keyVaultProvider = serviceProvider.GetService<IKeyVaultProvider>();
                var key = await keyVaultProvider?.GetSecretAsync(provider?.VaultKey);
                if (string.IsNullOrEmpty(key))
                {
                    throw new ApplicationException($"{provider?.VaultKey} key not found in Key Vault");
                }

                client.DefaultRequestHeaders.Add(provider.AuthHeader, key);
            }).AddStandardResilienceHandler(options =>
            {
                // Todo: Move to config
                options.Retry.MaxRetryAttempts = 5;
                options.Retry.BackoffType = DelayBackoffType.Exponential;
                options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(5);
                options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(15);
            });
        }
    })
    .Build();

host.Run();