using IcMarketsTestTask.API.Application.DTO;
using IcMarketsTestTask.API.Domain.Enums;

namespace IcMarketsTestTask.API.Application.Services.Blockchains;

public interface IBlockcypherClient
{
    Task<BlockchainSnapshotDto> GetSnapshotAsync(
        BlockchainNetwork network,
        CancellationToken cancellationToken
    );
}