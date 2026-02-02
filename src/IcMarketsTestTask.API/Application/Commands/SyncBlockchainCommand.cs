using IcMarketsTestTask.API.Domain.Enums;
using MediatR;

namespace IcMarketsTestTask.API.Application.Commands;

public record SyncBlockchainCommand(BlockchainNetwork Symbol) : IRequest;
