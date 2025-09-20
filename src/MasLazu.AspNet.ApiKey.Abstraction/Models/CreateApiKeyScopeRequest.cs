namespace MasLazu.AspNet.ApiKey.Abstraction.Models;

public record CreateApiKeyScopeRequest(
    Guid ApiKeyId,
    Guid PermissionId
);
