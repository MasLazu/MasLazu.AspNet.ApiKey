using MasLazu.AspNet.Framework.Application.Interfaces;
using MasLazu.AspNet.ApiKey.Abstraction.Models;

namespace MasLazu.AspNet.ApiKey.Abstraction.Interfaces;

public interface IApiKeyService : ICrudService<ApiKeyDto, CreateApiKeyRequest, UpdateApiKeyRequest>
{
    /// <summary>
    /// Revoke an API key by its Id.
    /// </summary>
    Task RevokeAsync(Guid userId, Guid apiKeyId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Rotate/regenerate the key value for an existing API key.
    /// </summary>
    Task<ApiKeyDto> RotateAsync(Guid userId, Guid apiKeyId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Validate an API key and optionally check for a permission.
    /// </summary>
    Task<bool> ValidateAsync(ValidateApiKeyRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Revoke all API keys for a user.
    /// </summary>
    Task RevokeAllForUserAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update the LastUsed property for an API key.
    /// </summary>
    Task UpdateLastUsedAsync(Guid userId, Guid apiKeyId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all API keys for a specific user.
    /// </summary>
    Task<IEnumerable<ApiKeyDto>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
