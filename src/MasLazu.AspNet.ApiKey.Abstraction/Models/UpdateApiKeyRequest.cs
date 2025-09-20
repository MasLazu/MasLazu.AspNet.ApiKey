using MasLazu.AspNet.Framework.Application.Models;

namespace MasLazu.AspNet.ApiKey.Abstraction.Models;

public record UpdateApiKeyRequest(
    Guid Id,
    string? Name,
    DateTime? ExpiresDate,
    DateTime? RevokedDate
) : BaseUpdateRequest(Id);
