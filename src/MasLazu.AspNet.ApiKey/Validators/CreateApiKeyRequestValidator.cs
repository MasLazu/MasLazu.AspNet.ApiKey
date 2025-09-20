using FluentValidation;
using MasLazu.AspNet.ApiKey.Abstraction.Models;

namespace MasLazu.AspNet.ApiKey.Validators;

public class CreateApiKeyRequestValidator : AbstractValidator<CreateApiKeyRequest>
{
    public CreateApiKeyRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .MaximumLength(100)
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.ExpiresDate)
            .GreaterThan(DateTime.UtcNow)
            .When(x => x.ExpiresDate.HasValue)
            .WithMessage("Expiration date must be in the future.");
    }
}
