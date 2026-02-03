using FluentAssertions;
using IcMarketsTestTask.API.Application.Extensions;
using IcMarketsTestTask.API.Application.Queries.Blockchains;
using IcMarketsTestTask.API.Domain.Entities;
using IcMarketsTestTask.API.Domain.Enums;
using IcMarketsTestTask.API.Infrastructure.Data;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace IcMarketsTestTask.UnitTests.Application.Queries.Blockchains;

public class GetBlockchainsQueryHandlerTests
{
    private readonly Mock<AppDbContext> _dbContextMock;
    private readonly GetBlockchainsQueryHandler _handler;

    public GetBlockchainsQueryHandlerTests()
    {
        _dbContextMock = new Mock<AppDbContext>();
        _handler = new GetBlockchainsQueryHandler(_dbContextMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnResult_When_BrokerHasLinks()
    {
        // Arrange
        BlockchainNetwork symbol = BlockchainNetwork.BtcMain;
        
        int limit = 10;
        var cancellationToken = new CancellationToken();
        var request = new GetBlockchainsQuery(symbol,  limit);

        var blockcypher = new Blockcypher()
        {
            Name = symbol.ToDbName()
        };
       
        _dbContextMock.Setup(x => x.Blockcyphers)
            .ReturnsDbSet(new List<Blockcypher>() { blockcypher });

        // Act
        var result = await _handler.Handle(request, cancellationToken);

        // Assert
        result.Should().HaveCount(1);
    }
}