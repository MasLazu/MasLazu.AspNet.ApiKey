using Microsoft.Extensions.DependencyInjection;
using MasLazu.AspNet.ApiKey.Validators;
using FluentValidation;
using MasLazu.AspNet.ApiKey.Abstraction.Models;

namespace MasLazu.AspNet.ApiKey.Extensions;

public static class ApiKeyApplicationValidatorExtension
{
    public static IServiceCollection AddApiKeyApplicationValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateApiKeyRequest>, CreateApiKeyRequestValidator>();
        services.AddScoped<IValidator<UpdateApiKeyRequest>, UpdateApiKeyRequestValidator>();
        services.AddScoped<IValidator<CreateApiKeyScopeRequest>, CreateApiKeyScopeRequestValidator>();
        services.AddScoped<IValidator<UpdateApiKeyScopeRequest>, UpdateApiKeyScopeRequestValidator>();

        return services;
    }
}
