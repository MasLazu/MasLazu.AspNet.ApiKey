using FluentValidation;
using MasLazu.AspNet.ApiKey.Abstraction.Models;

namespace MasLazu.AspNet.ApiKey.Validators;

public class CreateApiKeyScopeRequestValidator : AbstractValidator<CreateApiKeyScopeRequest>
{
    public CreateApiKeyScopeRequestValidator()
    {
        RuleFor(x => x.ApiKeyId)
            .NotEmpty();

        RuleFor(x => x.PermissionId)
            .NotEmpty();
    }
}
