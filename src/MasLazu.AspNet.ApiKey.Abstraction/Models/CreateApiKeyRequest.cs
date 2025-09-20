namespace MasLazu.AspNet.ApiKey.Abstraction.Models;

public record CreateApiKeyRequest(
    Guid UserId,
    string? Name,
    DateTime? ExpiresDate
);
