using AutoFixture;
using FluentAssertions;
using Forex.Partners.Api.IntegrationTests.Fixtures;
using IcMarketsTestTask.API.Application.Commands.Blockchains;
using IcMarketsTestTask.API.Application.Services.Blockchains;
using IcMarketsTestTask.API.Infrastructure.Data;
using IcMarketsTestTask.IntegrationTests.Fixtures;
using Xunit;

namespace IcMarketsTestTask.IntegrationTests.Tests.Application.Commands.Blockchains;

public class SyncBlockchainCommandHandlerTests : IntegrationTestBase, IClassFixture<DbContextFixture>, IClassFixture<AutoMapperFixture>
{
    public IServiceProvider ServiceProvider { get; }
    
    public IFixture Fixture { get; }

    public SyncBlockchainCommandHandler SutInstance =>
        ActivatorUtilities.CreateInstance<SyncBlockchainCommandHandler>(ServiceProvider);
    
    public SyncBlockchainCommandHandlerTests(
        DbContextFixture dbContextFixture,
        AutoMapperFixture mapperFixture) : base(dbContextFixture)
    {
        Fixture = new Fixture();
        
        var services = new ServiceCollection()
            .AddSingleton(_ => AppDbContext)
            .AddScoped(_ => mapperFixture.Mapper)
            .AddScoped<IBlockcypherClient, BlockcypherClient>();
        
        ServiceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public async Task Handle_When_PlatformClientNotExist_Should_ThrowNotFoundException()
    {
        // arrange
        var command = Fixture.Build<SyncBlockchainCommand>()
            .Create();
        
        // act
        var act =  async () => await SutInstance.Handle(command, CancellationToken.None);

        // assert
    }
}