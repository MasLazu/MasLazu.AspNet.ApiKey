using MasLazu.AspNet.Framework.Application.Interfaces;
using MasLazu.AspNet.ApiKey.Abstraction.Models;

namespace MasLazu.AspNet.ApiKey.Abstraction.Interfaces;

public interface IApiKeyScopeService : ICrudService<ApiKeyScopeDto, CreateApiKeyScopeRequest, UpdateApiKeyScopeRequest>
{
    /// <summary>
    /// Add a scope (permission) to an API key.
    /// </summary>
    Task<ApiKeyScopeDto> AddScopeAsync(Guid userId, ApiKeyScopeRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove a scope (permission) from an API key.
    /// </summary>
    Task RemoveScopeAsync(Guid userId, ApiKeyScopeRequest request, CancellationToken cancellationToken = default);
}
