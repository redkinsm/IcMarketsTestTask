using IcMarketsTestTask.API.Domain.Enums;

namespace IcMarketsTestTask.API.Application.Extensions;

public static class BlockchainNetworkExtensions
{
    public static string ToDbName(this BlockchainNetwork network) =>
        network switch
        {
            BlockchainNetwork.EthMain => "ETH.main",
            BlockchainNetwork.BtcMain => "BTC.main",
            BlockchainNetwork.BtcTest => "BTC.test3",
            BlockchainNetwork.LtcMain => "LTC.main",
            BlockchainNetwork.DashMain => "DASH.main",
            _ => throw new ArgumentOutOfRangeException(nameof(network))
        };
}