using AutoMapper;
using IcMarketsTestTask.API;
using Microsoft.Extensions.DependencyInjection;

namespace Forex.Partners.Api.IntegrationTests.Fixtures;

public class AutoMapperFixture
{
    public IMapper Mapper = BuildAutoMapper();

    private static IMapper BuildAutoMapper()
    {
        var serviceProvider = new ServiceCollection()
            .AddAutoMapper(typeof(AssemblyInfo).Assembly)
            .BuildServiceProvider();

        return serviceProvider.GetRequiredService<IMapper>();
    }
}
