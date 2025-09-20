using System.Linq.Expressions;
using MasLazu.AspNet.Framework.Application.Interfaces;
using MasLazu.AspNet.ApiKey.Domain.Entities;

namespace MasLazu.AspNet.ApiKey.Utils;

public class ApiKeyEntityPropertyMap : IEntityPropertyMap<Domain.Entities.ApiKey>
{
    private readonly Dictionary<string, Expression<Func<Domain.Entities.ApiKey, object>>> _map =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "id", ak => ak.Id },
            { "userId", ak => ak.UserId },
            { "key", ak => ak.Key },
            { "name", ak => ak.Name! },
            { "expiresDate", ak => ak.ExpiresDate! },
            { "lastUsed", ak => ak.LastUsed! },
            { "revokedDate", ak => ak.RevokedDate! },
            { "createdAt", ak => ak.CreatedAt },
            { "updatedAt", ak => ak.UpdatedAt! }
        };

    public Expression<Func<Domain.Entities.ApiKey, object>> Get(string property)
    {
        if (_map.TryGetValue(property, out Expression<Func<Domain.Entities.ApiKey, object>>? expr))
        {
            return expr;
        }

        throw new ArgumentException($"Property '{property}' is not supported for ApiKey. " +
            $"Supported properties: {string.Join(", ", _map.Keys)}");
    }
}
