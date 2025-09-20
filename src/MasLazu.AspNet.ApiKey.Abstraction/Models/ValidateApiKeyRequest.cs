namespace MasLazu.AspNet.ApiKey.Abstraction.Models;

public record ValidateApiKeyRequest(
    string Key,
    Guid? PermissionId
);
