using System.Linq.Expressions;
using MasLazu.AspNet.Framework.Application.Interfaces;
using MasLazu.AspNet.ApiKey.Domain.Entities;

namespace MasLazu.AspNet.ApiKey.Utils;

public class ApiKeyScopeEntityPropertyMap : IEntityPropertyMap<ApiKeyScope>
{
    private readonly Dictionary<string, Expression<Func<ApiKeyScope, object>>> _map =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "id", aks => aks.Id },
            { "apiKeyId", aks => aks.ApiKeyId },
            { "permissionId", aks => aks.PermissionId },
            { "createdAt", aks => aks.CreatedAt },
            { "updatedAt", aks => aks.UpdatedAt! }
        };

    public Expression<Func<ApiKeyScope, object>> Get(string property)
    {
        if (_map.TryGetValue(property, out Expression<Func<ApiKeyScope, object>>? expr))
        {
            return expr;
        }

        throw new ArgumentException($"Property '{property}' is not supported for ApiKeyScope. " +
            $"Supported properties: {string.Join(", ", _map.Keys)}");
    }
}
