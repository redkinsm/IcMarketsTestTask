using FluentValidation;
using IcMarketsTestTask.API.Application.Queries.Blockchains;

namespace IcMarketsTestTask.API.Application.Validators;

public class GetBlockchainsQueryValidator : AbstractValidator<GetBlockchainsQuery>
{
    public GetBlockchainsQueryValidator()
    {
        RuleFor(x => x.Symbol)
            .IsInEnum()
            .WithMessage("Unsupported blockchain network");
        
        RuleFor(x => x.Limit)
            .InclusiveBetween(1, 100)
            .WithMessage("Limit must be between 1 and 100");
    }
}
