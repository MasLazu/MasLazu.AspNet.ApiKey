using MasLazu.AspNet.Framework.Application.Models;

namespace MasLazu.AspNet.ApiKey.Abstraction.Models;

public record ApiKeyScopeDto(
    Guid Id,
    Guid ApiKeyId,
    Guid PermissionId,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt
) : BaseDto(Id, CreatedAt, UpdatedAt);
