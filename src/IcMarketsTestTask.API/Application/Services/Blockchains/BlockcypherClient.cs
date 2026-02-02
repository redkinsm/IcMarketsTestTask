using IcMarketsTestTask.API.Application.DTO;
using IcMarketsTestTask.API.Domain.Enums;

namespace IcMarketsTestTask.API.Application.Services.Blockchains;

public class BlockcypherClient(HttpClient http) : IBlockcypherClient
{
    private static readonly Dictionary<BlockchainNetwork, string> Paths =
        new()
        {
            [BlockchainNetwork.EthMain] = "eth/main",
            [BlockchainNetwork.BtcMain] = "btc/main",
            [BlockchainNetwork.BtcTest] = "btc/test3",
            [BlockchainNetwork.LtcMain] = "ltc/main",
            [BlockchainNetwork.DashMain] = "dash/main",
        };

    public async Task<BlockchainSnapshotDto> GetSnapshotAsync(
        BlockchainNetwork network,
        CancellationToken cancellationToken)
    {
        if (!Paths.TryGetValue(network, out var path))
            throw new ArgumentOutOfRangeException(nameof(network));

        var response = await http.GetAsync(path, cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<BlockchainSnapshotDto>(cancellationToken);
    }
}
