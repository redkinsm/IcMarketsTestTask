using AutoMapper;
using IcMarketsTestTask.API.Application.Services.Blockchains;
using IcMarketsTestTask.API.Domain.Entities;
using IcMarketsTestTask.API.Infrastructure.Data;
using MediatR;

namespace IcMarketsTestTask.API.Application.Commands.Blockchains;

public class SyncBlockchainCommandHandler(AppDbContext context, IMapper mapper, IBlockcypherClient blockcypherClient) : IRequestHandler<SyncBlockchainCommand>
{
    public async Task Handle(SyncBlockchainCommand request, CancellationToken cancellationToken)
    {
        var snapshotDto = await blockcypherClient.GetSnapshotAsync(request.Symbol, cancellationToken);
        var entity = mapper.Map<Blockcypher>(snapshotDto);
        
        await context.Blockcyphers.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}