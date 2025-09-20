using Microsoft.Extensions.DependencyInjection;

namespace MasLazu.AspNet.ApiKey.Extensions;

public static class ApiKeyApplicationExtension
{
    public static IServiceCollection AddApiKeyApplication(this IServiceCollection services)
    {
        services.AddApiKeyApplicationServices();
        services.AddApiKeyApplicationUtils();
        services.AddApiKeyApplicationValidators();

        return services;
    }
}
