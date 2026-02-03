using AutoFixture;
using FluentAssertions;
using Forex.Partners.Api.IntegrationTests.Fixtures;
using IcMarketsTestTask.API.Application.Extensions;
using IcMarketsTestTask.API.Application.Queries.Blockchains;
using IcMarketsTestTask.API.Domain.Entities;
using IcMarketsTestTask.IntegrationTests.Fixtures;
using Xunit;

namespace IcMarketsTestTask.IntegrationTests.Tests.Application.Queries.Blockchains;

public class GetBlockchainsQueryHandlerTests : IntegrationTestBase, IClassFixture<DbContextFixture>, IClassFixture<AutoMapperFixture>
{
    public IServiceProvider ServiceProvider { get; }
    
    public IFixture Fixture { get; }

    public GetBlockchainsQueryHandler SutInstance =>
        ActivatorUtilities.CreateInstance<GetBlockchainsQueryHandler>(ServiceProvider);
    
    public GetBlockchainsQueryHandlerTests(
        DbContextFixture dbContextFixture,
        AutoMapperFixture mapperFixture) : base(dbContextFixture)
    {
        Fixture = new Fixture();
        
        var services = new ServiceCollection()
            .AddSingleton(_ => AppDbContext)
            .AddScoped(_ => mapperFixture.Mapper);
        
        ServiceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public async Task Handle_When_BlockcyphersExist_Should_Return()
    {
        // arrange
        var query = Fixture.Build<GetBlockchainsQuery>()
            .With(x => x.Limit, 3)
            .Create();

        var blockcypher = Fixture.Build<Blockcypher>()
            .Without(x => x.Id)
            .With(x => x.Name, query.Symbol.ToDbName())
            .CreateMany(3).ToList();
        
        AppDbContext.Blockcyphers.AddRange(blockcypher);
        await AppDbContext.SaveChangesAsync();
        
        // act
        var result = await SutInstance.Handle(query, CancellationToken.None);

        // assert
        result.Count.Should().Be(3);
        result.Select(x => x.CreatedAt)
            .Should()
            .BeInDescendingOrder();
    }
    
    [Fact]
    public async Task Handle_When_BlockcyphersIsNotExist_Should_ReturnEmptyList()
    {
        // arrange
        var query = Fixture.Build<GetBlockchainsQuery>()
            .With(x => x.Limit, 3)
            .Create();

        var blockcypher = Fixture.Build<Blockcypher>()
            .Without(x => x.Id)
            .CreateMany(3).ToList();
        
        AppDbContext.Blockcyphers.AddRange(blockcypher);
        await AppDbContext.SaveChangesAsync();
        
        // act
        var result = await SutInstance.Handle(query, CancellationToken.None);

        // assert
        result.Count.Should().Be(0);
    }
    
    [Fact]
    public async Task Handle_When_BlockcyphersExistAndThereIsLimit_Should_ReturnLimitedList()
    {
        // arrange
        var query = Fixture.Build<GetBlockchainsQuery>()
            .With(x => x.Limit, 3)
            .Create();

        var blockcypher = Fixture.Build<Blockcypher>()
            .Without(x => x.Id)
            .With(x => x.Name, query.Symbol.ToDbName())
            .CreateMany(5).ToList();
        
        AppDbContext.Blockcyphers.AddRange(blockcypher);
        await AppDbContext.SaveChangesAsync();
        
        // act
        var result = await SutInstance.Handle(query, CancellationToken.None);

        // assert
        result.Count.Should().Be(3);
    }
}