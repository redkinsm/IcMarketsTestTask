using IcMarketsTestTask.API.Application.Services.Blockchains;
using IcMarketsTestTask.API.Domain.Enums;

namespace IcMarketsTestTask.API.ApiModels.Blockchains;

public class GetBlockchainsApiModel
{
    public BlockchainNetwork Symbol { get; set; }
    public int Limit { get; set; }
}