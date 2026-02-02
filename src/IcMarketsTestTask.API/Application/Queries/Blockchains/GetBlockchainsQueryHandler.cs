using IcMarketsTestTask.API.Application.Extensions;
using IcMarketsTestTask.API.Application.Services.Blockchains;
using IcMarketsTestTask.API.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IcMarketsTestTask.API.Application.Queries.Blockchains;

public class GetBlockchainsQueryHandler(AppDbContext context) : IRequestHandler<GetBlockchainsQuery, List<GetBlockchainsQueryResponse>>
{
    public async Task<List<GetBlockchainsQueryResponse>> Handle(GetBlockchainsQuery request, CancellationToken cancellationToken)
    {
        var res = await context.Blockcyphers
            .AsNoTracking()
            .Where(x => x.Name == request.Symbol.ToDbName())
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new GetBlockchainsQueryResponse(
                x.Name, x.Height, x.CreatedAt))
            .ToListAsync(cancellationToken);        
        return res;
    }
}