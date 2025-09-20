using FluentValidation;
using MasLazu.AspNet.ApiKey.Abstraction.Models;

namespace MasLazu.AspNet.ApiKey.Validators;

public class UpdateApiKeyScopeRequestValidator : AbstractValidator<UpdateApiKeyScopeRequest>
{
    public UpdateApiKeyScopeRequestValidator()
    {
        When(x => x.ApiKeyId != null, () => RuleFor(x => x.ApiKeyId!)
                .NotEmpty());

        When(x => x.PermissionId != null, () => RuleFor(x => x.PermissionId!)
                .NotEmpty());
    }
}
