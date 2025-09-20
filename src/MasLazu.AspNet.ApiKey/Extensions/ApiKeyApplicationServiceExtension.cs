using Microsoft.Extensions.DependencyInjection;
using MasLazu.AspNet.ApiKey.Services;
using MasLazu.AspNet.ApiKey.Abstraction.Interfaces;

namespace MasLazu.AspNet.ApiKey.Extensions;

public static class ApiKeyApplicationServiceExtension
{
    public static IServiceCollection AddApiKeyApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IApiKeyService, ApiKeyService>();
        services.AddScoped<IApiKeyScopeService, ApiKeyScopeService>();

        return services;
    }
}
