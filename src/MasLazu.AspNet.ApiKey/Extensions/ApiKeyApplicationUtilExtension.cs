using Microsoft.Extensions.DependencyInjection;
using MasLazu.AspNet.ApiKey.Utils;
using MasLazu.AspNet.ApiKey.Domain.Entities;
using MasLazu.AspNet.Framework.Application.Interfaces;
using MasLazu.AspNet.Framework.Application.Utils;

namespace MasLazu.AspNet.ApiKey.Extensions;

public static class ApiKeyApplicationUtilExtension
{
    public static IServiceCollection AddApiKeyApplicationUtils(this IServiceCollection services)
    {
        RegisterPropertyMapsAndExpressionBuilders(services);

        return services;
    }

    private static void RegisterPropertyMapsAndExpressionBuilders(IServiceCollection services)
    {
        var entityPropertyMapPairs = new (Type entityType, Type propertyMapType)[]
        {
            (typeof(Domain.Entities.ApiKey), typeof(ApiKeyEntityPropertyMap)),
            (typeof(ApiKeyScope), typeof(ApiKeyScopeEntityPropertyMap))
        };

        foreach ((Type entityType, Type propertyMapType) in entityPropertyMapPairs)
        {
            Type propertyMapInterfaceType = typeof(IEntityPropertyMap<>).MakeGenericType(entityType);
            services.AddSingleton(propertyMapInterfaceType, propertyMapType);

            Type expressionBuilderType = typeof(ExpressionBuilder<>).MakeGenericType(entityType);
            services.AddScoped(expressionBuilderType);
        }
    }
}
