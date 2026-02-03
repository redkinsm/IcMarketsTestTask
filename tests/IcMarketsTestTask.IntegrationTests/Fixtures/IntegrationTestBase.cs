using IcMarketsTestTask.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit;

namespace IcMarketsTestTask.IntegrationTests.Fixtures;

public abstract class IntegrationTestBase(DbContextFixture dbContextFixture) : IAsyncLifetime
{
    private IDbContextTransaction? Transaction { get; set; }
    protected AppDbContext? AppDbContext { get; private set; }

    public virtual async Task InitializeAsync()
    {
        AppDbContext = dbContextFixture.DbContext;

        const int maxRetries = 5;
        var delay = TimeSpan.FromSeconds(1);
        Exception? lastException = null;

        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            try
            {
                Transaction = await AppDbContext.Database.BeginTransactionAsync();
                return;
            }
            catch (Exception ex) when (attempt < maxRetries - 1)
            {
                lastException = ex;
                var isTimeout =
                    ex.Message.Contains("timeout", StringComparison.OrdinalIgnoreCase)
                    || ex.Message.Contains("not responding", StringComparison.OrdinalIgnoreCase)
                    || ex.GetType().Name.Contains("Timeout", StringComparison.OrdinalIgnoreCase);

                if (isTimeout)
                {
                    await Task.Delay(delay);
                    delay = TimeSpan.FromMilliseconds(delay.TotalMilliseconds * 1.5);
                }
                else
                {
                    throw;
                }
            }
        }

        throw lastException
            ?? new InvalidOperationException("Failed to begin transaction after retries");
    }

    public virtual async Task DisposeAsync()
    {
        if (Transaction != null)
        {
            try
            {
                await Transaction.RollbackAsync();
            }
            catch (InvalidOperationException) { }
            catch (Microsoft.Data.SqlClient.SqlException ex)
                when (ex.Message.Contains("completed") || ex.Message.Contains("no longer usable"))
            { }
            finally
            {
                await Transaction.DisposeAsync();
            }
        }
    }
}
