using MasLazu.AspNet.Framework.Application.Models;

namespace MasLazu.AspNet.ApiKey.Abstraction.Models;

public record UpdateApiKeyScopeRequest(
    Guid Id,
    Guid? ApiKeyId,
    Guid? PermissionId
) : BaseUpdateRequest(Id);
