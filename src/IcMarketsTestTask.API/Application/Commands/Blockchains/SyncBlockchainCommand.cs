using IcMarketsTestTask.API.Domain.Enums;
using MediatR;

namespace IcMarketsTestTask.API.Application.Commands.Blockchains;

public record SyncBlockchainCommand(BlockchainNetwork Symbol) : IRequest;
