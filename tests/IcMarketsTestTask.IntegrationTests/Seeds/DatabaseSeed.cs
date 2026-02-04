using IcMarketsTestTask.API.Infrastructure.Data;

namespace IcMarketsTestTask.IntegrationTests.Seeds;

public static class DatabaseSeed
{
    public static void EnsurePartnershipTypesV2Seed(AppDbContext dbContext)
    {
    }

    public static void EnsurePartnershipTypesSeed(AppDbContext dbContext)
    {
    }

    public static void EnsureBasicDataSeed(AppDbContext dbContext)
    {
        EnsurePartnershipTypesSeed(dbContext);
        EnsurePartnershipTypesV2Seed(dbContext);
    }
}
