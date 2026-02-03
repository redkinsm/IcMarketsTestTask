using IcMarketsTestTask.API.Infrastructure.Data;
using IcMarketsTestTask.IntegrationTests.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit;

namespace IcMarketsTestTask.IntegrationTests.Fixtures;

public class DbContextFixture : IAsyncLifetime
{
    private static readonly object Lock = new();
    private static bool _databaseInitialized;

    private readonly IServiceProvider _serviceProvider;

    public DbContextFixture()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var services = new ServiceCollection();

        var connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection string is not configured");

        if (
            !connectionString.Contains(
                "MultipleActiveResultSets",
                StringComparison.OrdinalIgnoreCase
            )
        )
        {
            var separator = connectionString.TrimEnd().EndsWith(";") ? "" : ";";
            connectionString = $"{connectionString}{separator}MultipleActiveResultSets=true";
        }

        if (!connectionString.Contains("Connection Timeout", StringComparison.OrdinalIgnoreCase))
        {
            var separator = connectionString.TrimEnd().EndsWith(";") ? "" : ";";
            connectionString = $"{connectionString}{separator}Connection Timeout=180";
        }

        services.AddDbContext<AppDbContext>(
            options =>
                options.UseSqlServer(
                    connectionString,
                    sqlServerOptions => sqlServerOptions.CommandTimeout(300)
                ),
            ServiceLifetime.Transient
        );

        _serviceProvider = services.BuildServiceProvider();

        lock (Lock)
        {
            if (!_databaseInitialized)
            {
                using (var context = DbContext)
                {
                    context.Database.EnsureCreated();

                    context.Database.ExecuteSqlRaw("ALTER DATABASE CURRENT SET RECOVERY SIMPLE");

                    DatabaseSeed.EnsureBasicDataSeed(context);
                }
                _databaseInitialized = true;
            }
        }
    }

    public AppDbContext DbContext => _serviceProvider.GetRequiredService<AppDbContext>();

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task DisposeAsync() { }
}
