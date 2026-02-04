using AutoMapper;
using IcMarketsTestTask.API.Application.Services.Blockchains;
using IcMarketsTestTask.API.Domain.Entities;
using IcMarketsTestTask.API.Infrastructure.Data;
using MediatR;

namespace IcMarketsTestTask.API.Application.Commands.Blockchains;

public class SyncBlockchainCommandHandler(AppDbContext context, IMapper mapper, ILogger<SyncBlockchainCommandHandler> logger, IBlockcypherClient blockcypherClient) : IRequestHandler<SyncBlockchainCommand>
{
    public async Task Handle(SyncBlockchainCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Sync blockchain started. Symbol: {Symbol}",
            request.Symbol
        );
        
        var snapshotDto = await blockcypherClient.GetSnapshotAsync(request.Symbol, cancellationToken);
        
        logger.LogInformation(
            "Snapshot received. Height: {Height}, Time: {Time}",
            snapshotDto.Height,
            snapshotDto.Time
        );
        
        var entity = mapper.Map<Blockcypher>(snapshotDto);
        
        await context.Blockcyphers.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}