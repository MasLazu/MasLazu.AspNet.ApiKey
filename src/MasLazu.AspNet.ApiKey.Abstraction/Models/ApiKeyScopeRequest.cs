namespace MasLazu.AspNet.ApiKey.Abstraction.Models;

public record ApiKeyScopeRequest(
    Guid ApiKeyId,
    Guid PermissionId
);
