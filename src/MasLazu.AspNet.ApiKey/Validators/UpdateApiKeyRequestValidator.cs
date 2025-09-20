using FluentValidation;
using MasLazu.AspNet.ApiKey.Abstraction.Models;

namespace MasLazu.AspNet.ApiKey.Validators;

public class UpdateApiKeyRequestValidator : AbstractValidator<UpdateApiKeyRequest>
{
        public UpdateApiKeyRequestValidator()
        {
                When(x => x.Name != null, () => RuleFor(x => x.Name!)
                        .MaximumLength(100));

                When(x => x.ExpiresDate != null, () => RuleFor(x => x.ExpiresDate!)
                        .GreaterThan(DateTime.UtcNow)
                        .WithMessage("Expiration date must be in the future."));

                When(x => x.RevokedDate != null, () => RuleFor(x => x.RevokedDate!)
                        .LessThanOrEqualTo(DateTime.UtcNow)
                        .WithMessage("Revocation date cannot be in the future."));
        }
}
