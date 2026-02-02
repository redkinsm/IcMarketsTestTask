using IcMarketsTestTask.API.Domain.Enums;
using MediatR;

namespace IcMarketsTestTask.API.Application.Queries.Blockchains;

public record GetBlockchainsQuery(BlockchainNetwork Symbol) : IRequest<List<GetBlockchainsQueryResponse>>;

public record GetBlockchainsQueryResponse(string Name, long Height, DateTime CreatedAt);