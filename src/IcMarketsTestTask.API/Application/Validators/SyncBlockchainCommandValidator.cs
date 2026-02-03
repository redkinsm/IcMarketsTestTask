using FluentValidation;
using IcMarketsTestTask.API.Application.Commands.Blockchains;

namespace IcMarketsTestTask.API.Application.Validators;

public class SyncBlockchainCommandValidator : AbstractValidator<SyncBlockchainCommand>
{
    public SyncBlockchainCommandValidator()
    {
        RuleFor(x => x.Symbol)
            .IsInEnum()
            .WithMessage("Unsupported blockchain network");
    }
}