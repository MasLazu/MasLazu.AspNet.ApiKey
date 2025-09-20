using Microsoft.Extensions.DependencyInjection;

namespace MasLazu.AspNet.ApiKey.EfCore.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiKeyEntityFrameworkCore(this IServiceCollection services)
    {
        return services;
    }
}
