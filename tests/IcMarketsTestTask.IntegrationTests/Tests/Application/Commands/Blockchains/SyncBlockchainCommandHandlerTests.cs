using AutoFixture;
using FluentAssertions;
using Forex.Partners.Api.IntegrationTests.Fixtures;
using IcMarketsTestTask.API.Application.Commands.Blockchains;
using IcMarketsTestTask.API.Application.DTO;
using IcMarketsTestTask.API.Application.Services.Blockchains;
using IcMarketsTestTask.API.Domain.Enums;
using IcMarketsTestTask.IntegrationTests.Fixtures;
using Xunit;
using Moq;
using NSubstitute;

namespace IcMarketsTestTask.IntegrationTests.Tests.Application.Commands.Blockchains;

public class SyncBlockchainCommandHandlerTests : IntegrationTestBase, IClassFixture<DbContextFixture>, IClassFixture<AutoMapperFixture>
{
    public IServiceProvider ServiceProvider { get; }
    
    public IFixture Fixture { get; }
    
    public Mock<IBlockcypherClient> BlockcypherClientMock { get; }
    
    public ILogger<SyncBlockchainCommandHandler> Logger { get; } = Substitute.For<ILogger<SyncBlockchainCommandHandler>>();

    public SyncBlockchainCommandHandler SutInstance =>
        ActivatorUtilities.CreateInstance<SyncBlockchainCommandHandler>(ServiceProvider);
    
    public SyncBlockchainCommandHandlerTests(
        DbContextFixture dbContextFixture,
        AutoMapperFixture mapperFixture) : base(dbContextFixture)
    {
        Fixture = new Fixture();
        
        BlockcypherClientMock = new Mock<IBlockcypherClient>();
        
        var services = new ServiceCollection()
            .AddSingleton(_ => AppDbContext)
            .AddScoped(_ => mapperFixture.Mapper)
            .AddScoped(_ => BlockcypherClientMock.Object)
            .AddSingleton(_ => Logger);
        
        ServiceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public async Task Handle_WhenSnapshotReceived_ShouldSaveBlockchainSnapshot()
    {
        // arrange
        var command = Fixture.Build<SyncBlockchainCommand>()
            .Create();

        var blockchainSnapshotDto = Fixture.Build<BlockchainSnapshotDto>()
            .Create();
        
        BlockcypherClientMock 
            .Setup(x => x.GetSnapshotAsync(
                It.IsAny<BlockchainNetwork>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(blockchainSnapshotDto);
        
        // act
        await SutInstance.Handle(command, CancellationToken.None);

        // assert
        
        var blockcypher = AppDbContext.Blockcyphers.FirstOrDefault(x => x.Name == blockchainSnapshotDto.Name);

        blockcypher.Should().NotBeNull();
        blockcypher.Name.Should().Be(blockchainSnapshotDto.Name);
        blockcypher.Height.Should().Be(blockchainSnapshotDto.Height);
        
    }
}